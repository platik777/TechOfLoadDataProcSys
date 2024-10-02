CREATE OR REPLACE FUNCTION UpdateUser(p_id INT, p_login VARCHAR, p_password VARCHAR, p_name VARCHAR, p_surname VARCHAR, p_age INT)
RETURNS VOID AS
$$
BEGIN
UPDATE Users
SET Login = p_login, Password = p_password, Name = p_name, Surname = p_surname, Age = p_age
WHERE Id = p_id;
END;
$$ LANGUAGE plpgsql;
