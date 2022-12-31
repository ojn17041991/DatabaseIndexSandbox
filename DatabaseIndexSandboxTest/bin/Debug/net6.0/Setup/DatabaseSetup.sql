-- We don't currently recreate the database each time the tests are run.
-- It needs to exist in advance, so use this command if you don't already have a test database.
--CREATE DATABASE index_sandbox_test;

-- Create the test tables.
CREATE TABLE IF NOT EXISTS users (id INT, first_name VARCHAR(20), last_name VARCHAR(20));
CREATE TABLE IF NOT EXISTS counter (id SERIAL);

-- Clear the test tables of all data before running any tests.
TRUNCATE TABLE users;
TRUNCATE TABLE counter;