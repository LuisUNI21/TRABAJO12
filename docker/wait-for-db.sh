#!/bin/bash
set -euo pipefail

# Robust wait-for-db script
# 1) Wait for DNS resolution of HOST
# 2) Wait for TCP port to be open
# 3) Optionally try sqlcmd if available

HOST="${DB_HOST:-db}"
PORT="${DB_PORT:-1433}"
TIMEOUT_SECONDS=${WAIT_FOR_DB_TIMEOUT:-300}
SLEEP_INTERVAL=${WAIT_FOR_DB_INTERVAL:-2}

echo "[wait-for-db] Waiting up to ${TIMEOUT_SECONDS}s for $HOST:$PORT..."
start=$(date +%s)

# Wait for name resolution
while true; do
  if getent hosts "$HOST" >/dev/null 2>&1 || ping -c 1 "$HOST" >/dev/null 2>&1; then
    echo "[wait-for-db] Host $HOST resolved"
    break
  fi
  now=$(date +%s)
  elapsed=$((now - start))
  if [ $elapsed -ge $TIMEOUT_SECONDS ]; then
    echo "[wait-for-db] Timeout waiting for DNS resolution of $HOST" >&2
    exit 1
  fi
  sleep $SLEEP_INTERVAL
done

# Wait for TCP port
while true; do
  if bash -c "</dev/tcp/$HOST/$PORT" >/dev/null 2>&1; then
    echo "[wait-for-db] TCP $HOST:$PORT reachable"
    break
  fi
  now=$(date +%s)
  elapsed=$((now - start))
  if [ $elapsed -ge $TIMEOUT_SECONDS ]; then
    echo "[wait-for-db] Timeout waiting for TCP $HOST:$PORT" >&2
    exit 1
  fi
  sleep $SLEEP_INTERVAL
done

# If sqlcmd is available, do a quick SELECT 1 to be extra sure
if command -v /opt/mssql-tools/bin/sqlcmd >/dev/null 2>&1 || command -v sqlcmd >/dev/null 2>&1; then
  SQLCMD=${SQLCMD:-/opt/mssql-tools/bin/sqlcmd}
  echo "[wait-for-db] Running a quick sqlcmd check..."
  if ! $SQLCMD -S "$HOST,$PORT" -U SA -P "${SA_PASSWORD:-Your_password123}" -Q "SELECT 1" >/dev/null 2>&1; then
    echo "[wait-for-db] sqlcmd check failed, but TCP is open. Continuing and letting app handle DB readiness..."
  else
    echo "[wait-for-db] sqlcmd check OK"
  fi
else
  echo "[wait-for-db] sqlcmd not available; skipped DB query check"
fi

echo "[wait-for-db] $HOST:$PORT is available - starting app"
exec dotnet TaskManager.Api.dll
