SELECT 'CREATE DATABASE "FinancialData" OWNER financegrid'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'FinancialData')\gexec

SELECT 'CREATE DATABASE "Users" OWNER financegrid'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'Users')\gexec

SELECT 'CREATE DATABASE "Webhook" OWNER financegrid'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'Webhook')\gexec
