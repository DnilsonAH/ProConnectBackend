-- Script para inicializar los roles en ProConnect
-- Ejecutar este script una sola vez después de crear la base de datos

USE b9rtb4siikyh6jpsdi8m;

-- Insertar roles si no existen
INSERT INTO `Role` (RoleName)
SELECT 'Admin' 
WHERE NOT EXISTS (SELECT 1 FROM `Role` WHERE RoleName = 'Admin');

INSERT INTO `Role` (RoleName)
SELECT 'Professional' 
WHERE NOT EXISTS (SELECT 1 FROM `Role` WHERE RoleName = 'Professional');

INSERT INTO `Role` (RoleName)
SELECT 'User' 
WHERE NOT EXISTS (SELECT 1 FROM `Role` WHERE RoleName = 'User');

-- Verificar roles insertados
SELECT * FROM `Role`;
