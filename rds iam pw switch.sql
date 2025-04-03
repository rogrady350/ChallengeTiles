ALTER USER 'admin' IDENTIFIED WITH AWSAuthenticationPlugin AS 'RDS';

SELECT user, host, plugin FROM mysql.user WHERE user = 'admin';