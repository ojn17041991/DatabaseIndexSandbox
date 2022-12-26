--DROP DATABASE IF EXISTS index_sandbox_test;
--CREATE DATABASE index_sandbox_test;

-- You can't select a database in Postgres without connecting, 
--  so index_sandbox_test will already exist (if not use the above commands).
-- You need to connect to index_sandbox_test and run the remaining commands.
DROP TABLE IF EXISTS users;
CREATE TABLE users (id INT, first_name VARCHAR(20), last_name VARCHAR(20));