### simple
```SQL
SELECT `c`.`Name` AS `Customer`, COALESCE(SUM(`o`.`Amount`), 0.0) AS `TotalAmount`
FROM `Orders` AS `o`
INNER JOIN `Customers` AS `c` ON `o`.`CustomerId` = `c`.`Id`
GROUP BY `c`.`Name`
```

### complex
```
SELECT `t`.`CustomerId`, `t`.`Name`, `t0`.`Category`, `t0`.`TotalAmount`
FROM (
    SELECT `o`.`CustomerId`, `c`.`Name`
    FROM `Orders` AS `o`
    INNER JOIN `Customers` AS `c` ON `o`.`CustomerId` = `c`.`Id`
    GROUP BY `o`.`CustomerId`, `c`.`Name`
) AS `t`
LEFT JOIN LATERAL (
    SELECT `o0`.`Category`, COALESCE(SUM(`o0`.`Amount`), 0.0) AS `TotalAmount`
    FROM `Orders` AS `o0`
    INNER JOIN `Customers` AS `c0` ON `o0`.`CustomerId` = `c0`.`Id`
    WHERE (`t`.`CustomerId` = `o0`.`CustomerId`) AND (`t`.`Name` = `c0`.`Name`)
    GROUP BY `o0`.`Category`
) AS `t0` ON TRUE
ORDER BY `t`.`CustomerId`, `t`.`Name`
```
