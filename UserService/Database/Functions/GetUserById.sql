CREATE OR REPLACE FUNCTION GetUserById(p_id INT)
RETURNS TABLE(id INT, login VARCHAR, password VARCHAR, name VARCHAR, surname VARCHAR, age INT) AS
$$
BEGIN
RETURN QUERY
SELECT Id, Login, Password, Name, Surname, Age
FROM users
WHERE Id = p_id;
END;
$$ LANGUAGE plpgsql;
