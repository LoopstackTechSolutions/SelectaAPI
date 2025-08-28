#!/bin/sh
set -e

host="$1"
shift
cmd="$@"

until mysql -h "$host" -P 3306 -u selecta -pselecta123 -e 'select 1' &> /dev/null; do
  echo "DB not ready yet. Retrying in 5s..."
  sleep 5
done

exec $cmd
