CREATE OR REPLACE FUNCTION GetUserBySurname(p_surname VARCHAR)
    RETURNS TABLE(id INT, login VARCHAR, password VARCHAR, name VARCHAR, surname VARCHAR, age INT) AS
$$
BEGIN
    RETURN QUERY
        SELECT *
        FROM users u
        WHERE u.Surname = p_surname;
END;
$$ LANGUAGE plpgsql;
