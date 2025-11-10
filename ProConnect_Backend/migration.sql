CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE TABLE `revoked_tokens` (
        `id` int NOT NULL AUTO_INCREMENT,
        `jti` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
        `revoked_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
        `expires_at` datetime NOT NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`id`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE TABLE `specialties` (
        `specialty_id` int unsigned NOT NULL AUTO_INCREMENT,
        `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
        `description` varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`specialty_id`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE TABLE `users` (
        `user_id` int unsigned NOT NULL AUTO_INCREMENT,
        `name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
        `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
        `password_hash` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
        `phone_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
        `registration_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
        `role` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
        `photo_url` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`user_id`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE TABLE `professional_profiles` (
        `profile_id` int unsigned NOT NULL AUTO_INCREMENT,
        `user_id` int unsigned NOT NULL,
        `specialty_id` int unsigned NOT NULL,
        `experience` varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
        `headline` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`profile_id`),
        CONSTRAINT `professional_profiles_specialty_id_foreign` FOREIGN KEY (`specialty_id`) REFERENCES `specialties` (`specialty_id`),
        CONSTRAINT `professional_profiles_user_id_foreign` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE TABLE `sessions` (
        `session_id` int unsigned NOT NULL AUTO_INCREMENT,
        `start_date` datetime NOT NULL,
        `end_date` datetime NOT NULL,
        `professional_id` int unsigned NOT NULL,
        `client_id` int unsigned NOT NULL,
        `meet_url` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
        `status` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`session_id`),
        CONSTRAINT `sessions_client_id_foreign` FOREIGN KEY (`client_id`) REFERENCES `users` (`user_id`),
        CONSTRAINT `sessions_professional_id_foreign` FOREIGN KEY (`professional_id`) REFERENCES `users` (`user_id`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE TABLE `verifications` (
        `verification_id` int unsigned NOT NULL AUTO_INCREMENT,
        `user_id` int unsigned NOT NULL,
        `status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'Pending',
        `verification_date` datetime NULL,
        `notes` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
        `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`verification_id`),
        CONSTRAINT `verifications_user_id_foreign` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE TABLE `weekly_availabilities` (
        `weekly_availability_id` int unsigned NOT NULL AUTO_INCREMENT,
        `professional_id` int unsigned NOT NULL,
        `start_date_time` datetime NOT NULL,
        `end_date_time` datetime NOT NULL,
        `week_day` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'DEFAULT TRUE',
        CONSTRAINT `PRIMARY` PRIMARY KEY (`weekly_availability_id`),
        CONSTRAINT `weekly_availabilities_professional_id_foreign` FOREIGN KEY (`professional_id`) REFERENCES `users` (`user_id`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE TABLE `payments` (
        `payment_id` int unsigned NOT NULL AUTO_INCREMENT,
        `session_id` int unsigned NOT NULL,
        `total_amount` decimal(10,2) NOT NULL,
        `status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'Pending',
        `method` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
        `payment_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`payment_id`),
        CONSTRAINT `payments_session_id_foreign` FOREIGN KEY (`session_id`) REFERENCES `sessions` (`session_id`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE TABLE `reviews` (
        `review_id` int unsigned NOT NULL AUTO_INCREMENT,
        `session_id` int unsigned NOT NULL,
        `client_id` int unsigned NOT NULL,
        `professional_id` int unsigned NOT NULL,
        `rating` decimal(2,2) NOT NULL,
        `comment` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
        `review_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`review_id`),
        CONSTRAINT `reviews_client_id_foreign` FOREIGN KEY (`client_id`) REFERENCES `users` (`user_id`),
        CONSTRAINT `reviews_professional_id_foreign` FOREIGN KEY (`professional_id`) REFERENCES `users` (`user_id`),
        CONSTRAINT `reviews_session_id_foreign` FOREIGN KEY (`session_id`) REFERENCES `sessions` (`session_id`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE TABLE `scheduleds` (
        `availability_id` int unsigned NOT NULL AUTO_INCREMENT,
        `session_id` int unsigned NOT NULL,
        `start_date` datetime NOT NULL,
        `end_date` datetime NOT NULL,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`availability_id`),
        CONSTRAINT `scheduleds_session_id_foreign` FOREIGN KEY (`session_id`) REFERENCES `sessions` (`session_id`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE TABLE `verification_documents` (
        `document_id` int unsigned NOT NULL AUTO_INCREMENT,
        `verification_id` int unsigned NOT NULL,
        `document_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
        `file_url` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
        `uploaded_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
        CONSTRAINT `PRIMARY` PRIMARY KEY (`document_id`),
        CONSTRAINT `verification_documents_verification_id_foreign` FOREIGN KEY (`verification_id`) REFERENCES `verifications` (`verification_id`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `payments_payment_date_index` ON `payments` (`payment_date`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `payments_session_id_index` ON `payments` (`session_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `payments_status_index` ON `payments` (`status`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `payments_status_payment_date_index` ON `payments` (`status`, `payment_date`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `professional_profiles_specialty_id_index` ON `professional_profiles` (`specialty_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE UNIQUE INDEX `professional_profiles_user_id_unique` ON `professional_profiles` (`user_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `reviews_client_id_index` ON `reviews` (`client_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `reviews_professional_id_index` ON `reviews` (`professional_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `reviews_rating_index` ON `reviews` (`rating`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `reviews_review_date_index` ON `reviews` (`review_date`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `reviews_session_id_foreign` ON `reviews` (`session_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `revoked_tokens_expires_at_index` ON `revoked_tokens` (`expires_at`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE UNIQUE INDEX `revoked_tokens_jti_unique` ON `revoked_tokens` (`jti`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `scheduleds_session_id_foreign` ON `scheduleds` (`session_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `sessions_client_id_foreign` ON `sessions` (`client_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `sessions_professional_id_foreign` ON `sessions` (`professional_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE UNIQUE INDEX `specialties_name_unique` ON `specialties` (`name`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE UNIQUE INDEX `users_email_unique` ON `users` (`email`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `users_registration_date_index` ON `users` (`registration_date`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `verification_documents_document_type_index` ON `verification_documents` (`document_type`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `verification_documents_uploaded_at_index` ON `verification_documents` (`uploaded_at`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `verification_documents_verification_id_index` ON `verification_documents` (`verification_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `verifications_created_at_index` ON `verifications` (`created_at`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `verifications_status_index` ON `verifications` (`status`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `verifications_user_id_index` ON `verifications` (`user_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `verifications_verification_date_index` ON `verifications` (`verification_date`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `professional_id_start_date_time_end_date_time_index` ON `weekly_availabilities` (`professional_id`, `start_date_time`, `end_date_time`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `weekly_availabilities_end_date_time_index` ON `weekly_availabilities` (`end_date_time`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `weekly_availabilities_professional_id_index` ON `weekly_availabilities` (`professional_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `weekly_availabilities_start_date_time_index` ON `weekly_availabilities` (`start_date_time`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    CREATE INDEX `weekly_availabilities_week_day_index` ON `weekly_availabilities` (`week_day`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251110195305_AddRevokedTokensTable') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251110195305_AddRevokedTokensTable', '9.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

