CREATE OR REPLACE FUNCTION GetUserByName(p_name VARCHAR)
    RETURNS TABLE(id INT, login VARCHAR, password VARCHAR, name VARCHAR, surname VARCHAR, age INT) AS
$$
BEGIN
    RETURN QUERY
        SELECT *
        FROM users u
        WHERE u.Name = p_name;
END;
$$ LANGUAGE plpgsql;
