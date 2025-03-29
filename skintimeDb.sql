-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: SkinTimeDb
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `AggregatedCounter`
--

DROP TABLE IF EXISTS `AggregatedCounter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AggregatedCounter` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` int NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_CounterAggregated_Key` (`Key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AggregatedCounter`
--

LOCK TABLES `AggregatedCounter` WRITE;
/*!40000 ALTER TABLE `AggregatedCounter` DISABLE KEYS */;
/*!40000 ALTER TABLE `AggregatedCounter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Bookings`
--

DROP TABLE IF EXISTS `Bookings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Bookings` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `customer_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `therapist_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `service_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `voucher_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `TotalPrice` decimal(65,30) NOT NULL,
  `reserved_date` datetime(6) NOT NULL,
  `total_payment` decimal(65,30) NOT NULL,
  `booking_status` int NOT NULL,
  `transaction_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_Bookings_transaction_id` (`transaction_id`),
  KEY `IX_Bookings_customer_id` (`customer_id`),
  KEY `IX_Bookings_service_id` (`service_id`),
  KEY `IX_Bookings_therapist_id` (`therapist_id`),
  KEY `IX_Bookings_voucher_id` (`voucher_id`),
  CONSTRAINT `FK_Bookings_Services_service_id` FOREIGN KEY (`service_id`) REFERENCES `Services` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Bookings_Therapists_therapist_id` FOREIGN KEY (`therapist_id`) REFERENCES `Therapists` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Bookings_Transactions_transaction_id` FOREIGN KEY (`transaction_id`) REFERENCES `Transactions` (`id`),
  CONSTRAINT `FK_Bookings_Users_customer_id` FOREIGN KEY (`customer_id`) REFERENCES `Users` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Bookings_Vouchers_voucher_id` FOREIGN KEY (`voucher_id`) REFERENCES `Vouchers` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Bookings`
--

LOCK TABLES `Bookings` WRITE;
/*!40000 ALTER TABLE `Bookings` DISABLE KEYS */;
/*!40000 ALTER TABLE `Bookings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Counter`
--

DROP TABLE IF EXISTS `Counter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Counter` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` int NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Counter_Key` (`Key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Counter`
--

LOCK TABLES `Counter` WRITE;
/*!40000 ALTER TABLE `Counter` DISABLE KEYS */;
/*!40000 ALTER TABLE `Counter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DistributedLock`
--

DROP TABLE IF EXISTS `DistributedLock`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DistributedLock` (
  `Resource` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DistributedLock`
--

LOCK TABLES `DistributedLock` WRITE;
/*!40000 ALTER TABLE `DistributedLock` DISABLE KEYS */;
/*!40000 ALTER TABLE `DistributedLock` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `EventTickets`
--

DROP TABLE IF EXISTS `EventTickets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `EventTickets` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `paid_amount` decimal(16,2) NOT NULL,
  `qr_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ticket_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ticket_status` int NOT NULL,
  `user_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `event_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `transaction_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_EventTickets_transaction_id` (`transaction_id`),
  KEY `IX_EventTickets_event_id` (`event_id`),
  KEY `IX_EventTickets_user_id` (`user_id`),
  CONSTRAINT `FK_EventTickets_Events_event_id` FOREIGN KEY (`event_id`) REFERENCES `Events` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_EventTickets_Transactions_transaction_id` FOREIGN KEY (`transaction_id`) REFERENCES `Transactions` (`id`),
  CONSTRAINT `FK_EventTickets_Users_user_id` FOREIGN KEY (`user_id`) REFERENCES `Users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `EventTickets`
--

LOCK TABLES `EventTickets` WRITE;
/*!40000 ALTER TABLE `EventTickets` DISABLE KEYS */;
/*!40000 ALTER TABLE `EventTickets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Events`
--

DROP TABLE IF EXISTS `Events`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Events` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `event_name` varchar(120) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `capacity` int NOT NULL,
  `ticket_price` decimal(16,2) NOT NULL,
  `description` varchar(250) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `eventdate` date NOT NULL,
  `time_start` time NOT NULL,
  `time_end` time NOT NULL,
  `location` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `thumbnail_url` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `status` int NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Events`
--

LOCK TABLES `Events` WRITE;
/*!40000 ALTER TABLE `Events` DISABLE KEYS */;
INSERT INTO `Events` VALUES ('08dd6e4f-b752-4084-8fd4-5e3a49078b75','Skincare Workshop',50,150000.00,'Hội thảo về chăm sóc da với chuyên gia','2025-04-10','14:00:00','17:00:00','Hà Nội','https://www.gotmink.pt/wp-content/uploads/2020/11/capa-930x620.jpg',1,'2025-03-29 06:24:41.353621','2025-03-29 06:24:41.353623'),('08dd6e4f-b754-43c2-8a02-d466a539ed1f','Anti-aging Techniques',40,200000.00,'Bí quyết chống lão hóa cho làn da tươi trẻ','2025-05-12','10:00:00','13:00:00','TP. HCM','https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSEjJbv-NQ8ASDd8fxFyq6yWWKAdF2Xf7dpFA&s',1,'2025-03-29 06:24:41.354289','2025-03-29 06:24:41.354289'),('08dd6e4f-b754-43e6-8f77-333686eb3233','Natural Skincare',60,180000.00,'Chăm sóc da bằng nguyên liệu thiên nhiên','2025-06-15','09:00:00','12:00:00','Đà Nẵng','https://d1csarkz8obe9u.cloudfront.net/posterpreviews/skin-care-flyers-design-template-57ce05f7cff5d8db4675a1034a4d9712_screen.jpg?ts=1621586739',1,'2025-03-29 06:24:41.354297','2025-03-29 06:24:41.354297'),('08dd6e4f-b754-43f1-8993-2084acd6d734','Sun Protection Seminar',30,120000.00,'Cách bảo vệ da khỏi tác hại của ánh nắng mặt trời','2025-07-20','15:00:00','17:30:00','Hà Nội','https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS_oCNbN6xQUVb1NzR9WSXZvWnAAVjsZjWUag&s',1,'2025-03-29 06:24:41.354299','2025-03-29 06:24:41.354299'),('08dd6e4f-b754-43fd-8ad6-48e09e47c885','Acne Treatment Session',25,100000.00,'Hướng dẫn trị mụn hiệu quả và ngăn ngừa tái phát','2025-08-05','16:00:00','18:30:00','TP. HCM','https://www.laroche-posay.us/dw/image/v2/AANG_PRD/on/demandware.static/-/Sites-lrp-us-Library/default/dw02903768/pages/Acne%20Positivity%20LP/AcnePos_CP_CRSL_Banner2_MOB_640x650.jpg?sw=640&sh=650&sm=cut&q=70',0,'2025-03-29 06:24:41.354300','2025-03-29 06:24:41.354300'),('08dd6e4f-b754-4407-81d8-1093b2b465e0','Night Skincare Routine',35,170000.00,'Quy trình chăm sóc da buổi tối cho làn da khỏe mạnh','2025-09-10','18:00:00','21:00:00','Hải Phòng','https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSYpce7c_1Gqrk-KATJOc0SnO0mKmxI-xQqksUBZgseSvgsF9-3tCVY9qlOYTEVi7UTWsI&usqp=CAU',1,'2025-03-29 06:24:41.354302','2025-03-29 06:24:41.354302'),('08dd6e4f-b754-4411-8064-fa1f8a78cdee','Hydration & Moisturizing',50,160000.00,'Dưỡng ẩm đúng cách cho từng loại da','2025-10-05','11:00:00','14:00:00','Cần Thơ','https://cdn.shopify.com/s/files/1/0508/8860/5891/files/MODULO_10_10.png?v=1729625168',1,'2025-03-29 06:24:41.354304','2025-03-29 06:24:41.354304'),('08dd6e4f-b754-441b-87f2-4c32e9a51572','Skincare for Men',45,140000.00,'Hướng dẫn chăm sóc da dành riêng cho nam giới','2025-11-12','13:00:00','16:00:00','Hà Nội','https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTwAxwStJIZmygTFzDs5iF6HaHGJh9Utj575A&s',1,'2025-03-29 06:24:41.354305','2025-03-29 06:24:41.354305'),('08dd6e4f-b754-4424-8db0-317f4a269212','Seasonal Skincare Tips',55,190000.00,'Cách chăm sóc da theo từng mùa','2025-12-20','09:00:00','11:30:00','TP. HCM','https://www.botanicalscience.net/wp-content/uploads/2024/08/Seasonal-Skincare-Feature-Image_240805.jpg',0,'2025-03-29 06:24:41.354307','2025-03-29 06:24:41.354307'),('08dd6e4f-b754-442d-8da5-ecacbaca1f9b','Skincare & Makeup',70,220000.00,'Kết hợp chăm sóc da và trang điểm tự nhiên','2026-01-15','10:30:00','14:30:00','Đà Nẵng','https://i.ytimg.com/vi/kPOSNVrSadI/maxresdefault.jpg',1,'2025-03-29 06:24:41.354308','2025-03-29 06:24:41.354308');
/*!40000 ALTER TABLE `Events` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Feedbacks`
--

DROP TABLE IF EXISTS `Feedbacks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Feedbacks` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `booking_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `therapist_rating` int NOT NULL,
  `service_rating` int NOT NULL,
  `therapist_feedback` varchar(250) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `service_feedback` varchar(250) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_Feedbacks_booking_id` (`booking_id`),
  CONSTRAINT `FK_Feedbacks_Bookings_booking_id` FOREIGN KEY (`booking_id`) REFERENCES `Bookings` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Feedbacks`
--

LOCK TABLES `Feedbacks` WRITE;
/*!40000 ALTER TABLE `Feedbacks` DISABLE KEYS */;
/*!40000 ALTER TABLE `Feedbacks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Hash`
--

DROP TABLE IF EXISTS `Hash`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Hash` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Field` varchar(40) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` longtext,
  `ExpireAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Hash_Key_Field` (`Key`,`Field`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Hash`
--

LOCK TABLES `Hash` WRITE;
/*!40000 ALTER TABLE `Hash` DISABLE KEYS */;
/*!40000 ALTER TABLE `Hash` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Job`
--

DROP TABLE IF EXISTS `Job`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Job` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StateId` int DEFAULT NULL,
  `StateName` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `InvocationData` longtext NOT NULL,
  `Arguments` longtext NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `ExpireAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Job_StateName` (`StateName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Job`
--

LOCK TABLES `Job` WRITE;
/*!40000 ALTER TABLE `Job` DISABLE KEYS */;
/*!40000 ALTER TABLE `Job` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `JobParameter`
--

DROP TABLE IF EXISTS `JobParameter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `JobParameter` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `Name` varchar(40) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_JobParameter_JobId_Name` (`JobId`,`Name`),
  KEY `FK_JobParameter_Job` (`JobId`),
  CONSTRAINT `FK_JobParameter_Job` FOREIGN KEY (`JobId`) REFERENCES `Job` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `JobParameter`
--

LOCK TABLES `JobParameter` WRITE;
/*!40000 ALTER TABLE `JobParameter` DISABLE KEYS */;
/*!40000 ALTER TABLE `JobParameter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `JobQueue`
--

DROP TABLE IF EXISTS `JobQueue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `JobQueue` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `FetchedAt` datetime(6) DEFAULT NULL,
  `Queue` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `FetchToken` varchar(36) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_JobQueue_QueueAndFetchedAt` (`Queue`,`FetchedAt`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `JobQueue`
--

LOCK TABLES `JobQueue` WRITE;
/*!40000 ALTER TABLE `JobQueue` DISABLE KEYS */;
/*!40000 ALTER TABLE `JobQueue` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `JobState`
--

DROP TABLE IF EXISTS `JobState`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `JobState` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Name` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Reason` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `Data` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_JobState_Job` (`JobId`),
  CONSTRAINT `FK_JobState_Job` FOREIGN KEY (`JobId`) REFERENCES `Job` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `JobState`
--

LOCK TABLES `JobState` WRITE;
/*!40000 ALTER TABLE `JobState` DISABLE KEYS */;
/*!40000 ALTER TABLE `JobState` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `List`
--

DROP TABLE IF EXISTS `List`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `List` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` longtext,
  `ExpireAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `List`
--

LOCK TABLES `List` WRITE;
/*!40000 ALTER TABLE `List` DISABLE KEYS */;
/*!40000 ALTER TABLE `List` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Messages`
--

DROP TABLE IF EXISTS `Messages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Messages` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `content` varchar(2048) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `is_read` tinyint(1) NOT NULL,
  `to_user_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `reaction` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `user_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_Messages_user_id` (`user_id`),
  CONSTRAINT `FK_Messages_Users_user_id` FOREIGN KEY (`user_id`) REFERENCES `Users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Messages`
--

LOCK TABLES `Messages` WRITE;
/*!40000 ALTER TABLE `Messages` DISABLE KEYS */;
/*!40000 ALTER TABLE `Messages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Notifications`
--

DROP TABLE IF EXISTS `Notifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Notifications` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `content` varchar(2048) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `to_user_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `is_read` tinyint(1) NOT NULL,
  `about_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `user_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_Notifications_user_id` (`user_id`),
  CONSTRAINT `FK_Notifications_Users_user_id` FOREIGN KEY (`user_id`) REFERENCES `Users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Notifications`
--

LOCK TABLES `Notifications` WRITE;
/*!40000 ALTER TABLE `Notifications` DISABLE KEYS */;
/*!40000 ALTER TABLE `Notifications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `QuestionOptionSkinType`
--

DROP TABLE IF EXISTS `QuestionOptionSkinType`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `QuestionOptionSkinType` (
  `QuestionOptionsId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `SkinTypesId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`QuestionOptionsId`,`SkinTypesId`),
  KEY `IX_QuestionOptionSkinType_SkinTypesId` (`SkinTypesId`),
  CONSTRAINT `FK_QuestionOptionSkinType_QuestionOptions_QuestionOptionsId` FOREIGN KEY (`QuestionOptionsId`) REFERENCES `QuestionOptions` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_QuestionOptionSkinType_SkinTypes_SkinTypesId` FOREIGN KEY (`SkinTypesId`) REFERENCES `SkinTypes` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `QuestionOptionSkinType`
--

LOCK TABLES `QuestionOptionSkinType` WRITE;
/*!40000 ALTER TABLE `QuestionOptionSkinType` DISABLE KEYS */;
INSERT INTO `QuestionOptionSkinType` VALUES ('08dd6e4f-b74d-464b-87ad-12865168ae0e','099717de-a077-4324-88c1-9a969450e3e3'),('08dd6e4f-b74d-4744-8d7a-5e1fc5053764','099717de-a077-4324-88c1-9a969450e3e3'),('08dd6e4f-b74d-492a-84ba-1f1067b827b0','099717de-a077-4324-88c1-9a969450e3e3'),('08dd6e4f-b74b-47fb-81b3-e0beb6981a5a','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b74d-4605-884a-36ea5ca99d25','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b74d-46b4-8351-36925e5c8aae','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b74d-46fd-8b4d-ef3f19acb46a','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b74d-4744-8d7a-5e1fc5053764','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b74d-47d4-8298-f7613f13b5fd','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b74d-48ad-8014-b06e3d79d2b3','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b74d-492a-84ba-1f1067b827b0','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b74d-449b-834d-715802104466','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b74d-457a-863f-d4541d57305f','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b74d-45bd-8dd5-fe30dba640f0','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b74d-4744-8d7a-5e1fc5053764','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b74d-4817-8ceb-1d24829ca1ce','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b74d-48eb-8993-d857ce626325','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b74d-492a-84ba-1f1067b827b0','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b74d-49a7-8808-a1599688113e','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b74d-44ef-84d7-7c8c9e6a9125','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b74d-457a-863f-d4541d57305f','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b74d-45bd-8dd5-fe30dba640f0','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b74d-467e-8ebd-fa3644737264','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b74d-4744-8d7a-5e1fc5053764','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b74d-4817-8ceb-1d24829ca1ce','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b74d-48eb-8993-d857ce626325','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b74d-492a-84ba-1f1067b827b0','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b74d-49a7-8808-a1599688113e','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b74d-451d-807f-f85e54b55505','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b74d-4605-884a-36ea5ca99d25','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b74d-46b4-8351-36925e5c8aae','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b74d-46fd-8b4d-ef3f19acb46a','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b74d-4744-8d7a-5e1fc5053764','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b74d-47d4-8298-f7613f13b5fd','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b74d-4859-87dc-ca1cd5e1656a','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b74d-4885-8601-66dc57d22ea2','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b74d-48ad-8014-b06e3d79d2b3','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b74d-492a-84ba-1f1067b827b0','b2662db8-eb00-491b-b409-94317ad5a8b7');
/*!40000 ALTER TABLE `QuestionOptionSkinType` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `QuestionOptions`
--

DROP TABLE IF EXISTS `QuestionOptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `QuestionOptions` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `content` varchar(250) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `question_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_QuestionOptions_question_id` (`question_id`),
  CONSTRAINT `FK_QuestionOptions_Questions_question_id` FOREIGN KEY (`question_id`) REFERENCES `Questions` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `QuestionOptions`
--

LOCK TABLES `QuestionOptions` WRITE;
/*!40000 ALTER TABLE `QuestionOptions` DISABLE KEYS */;
INSERT INTO `QuestionOptions` VALUES ('08dd6e4f-b74b-47fb-81b3-e0beb6981a5a','A. Khô, bình tĩnh và dễ chăm sóc','08dd6e4f-b741-4da4-844e-679be0903eff','2025-03-29 06:24:41.311182','2025-03-29 06:24:41.311184'),('08dd6e4f-b74d-449b-834d-715802104466','B. Bóng, nhờn và có một chút vấn đề','08dd6e4f-b741-4da4-844e-679be0903eff','2025-03-29 06:24:41.311296','2025-03-29 06:24:41.311296'),('08dd6e4f-b74d-44ef-84d7-7c8c9e6a9125','C. Vùng trán và mũi của tôi trơn','08dd6e4f-b741-4da4-844e-679be0903eff','2025-03-29 06:24:41.311301','2025-03-29 06:24:41.311301'),('08dd6e4f-b74d-451d-807f-f85e54b55505','D. Căng sau khi tôi rửa bằng chất tẩy rửa không tự nhiên','08dd6e4f-b741-4da4-844e-679be0903eff','2025-03-29 06:24:41.311301','2025-03-29 06:24:41.311301'),('08dd6e4f-b74d-457a-863f-d4541d57305f','A. Luôn luôn','08dd6e4f-b743-46bc-8b3e-a7a5af025596','2025-03-29 06:24:41.311301','2025-03-29 06:24:41.311301'),('08dd6e4f-b74d-45bd-8dd5-fe30dba640f0','B. Rất hiếm khi','08dd6e4f-b743-46bc-8b3e-a7a5af025596','2025-03-29 06:24:41.311301','2025-03-29 06:24:41.311301'),('08dd6e4f-b74d-4605-884a-36ea5ca99d25','C. Vào thời điểm kinh nguyệt của tôi','08dd6e4f-b743-46bc-8b3e-a7a5af025596','2025-03-29 06:24:41.311302','2025-03-29 06:24:41.311302'),('08dd6e4f-b74d-464b-87ad-12865168ae0e','D. Thỉnh thoảng','08dd6e4f-b743-46bc-8b3e-a7a5af025596','2025-03-29 06:24:41.311302','2025-03-29 06:24:41.311302'),('08dd6e4f-b74d-467e-8ebd-fa3644737264','A. Trên trán, dọc theo đường chân tóc và trên cằm','08dd6e4f-b743-46e0-8ba0-91ac91aa1b22','2025-03-29 06:24:41.311303','2025-03-29 06:24:41.311303'),('08dd6e4f-b74d-46b4-8351-36925e5c8aae','B. Rất hiếm khi','08dd6e4f-b743-46e0-8ba0-91ac91aa1b22','2025-03-29 06:24:41.311303','2025-03-29 06:24:41.311303'),('08dd6e4f-b74d-46fd-8b4d-ef3f19acb46a','C. Thường là khi tôi không rửa mặt bằng chất tẩy rửa tự nhiên','08dd6e4f-b743-46e0-8ba0-91ac91aa1b22','2025-03-29 06:24:41.311303','2025-03-29 06:24:41.311303'),('08dd6e4f-b74d-4744-8d7a-5e1fc5053764','D. Một lần một tháng','08dd6e4f-b743-46e0-8ba0-91ac91aa1b22','2025-03-29 06:24:41.311303','2025-03-29 06:24:41.311304'),('08dd6e4f-b74d-47d4-8298-f7613f13b5fd','A. Không có mụn','08dd6e4f-b743-46ea-89dc-29c5e20489bf','2025-03-29 06:24:41.311304','2025-03-29 06:24:41.311304'),('08dd6e4f-b74d-4817-8ceb-1d24829ca1ce','B. Cảm thấy sạch','08dd6e4f-b743-46ea-89dc-29c5e20489bf','2025-03-29 06:24:41.311305','2025-03-29 06:24:41.311305'),('08dd6e4f-b74d-4859-87dc-ca1cd5e1656a','C. Không đỏ và viêm','08dd6e4f-b743-46ea-89dc-29c5e20489bf','2025-03-29 06:24:41.311306','2025-03-29 06:24:41.311306'),('08dd6e4f-b74d-4885-8601-66dc57d22ea2','D. Trông khỏe mạnh','08dd6e4f-b743-46ea-89dc-29c5e20489bf','2025-03-29 06:24:41.311306','2025-03-29 06:24:41.311306'),('08dd6e4f-b74d-48ad-8014-b06e3d79d2b3','A. Tôi sử dụng các sản phẩm chăm sóc da tự nhiên hai lần một ngày','08dd6e4f-b743-46f5-8d84-7e02ea48e398','2025-03-29 06:24:41.311306','2025-03-29 06:24:41.311306'),('08dd6e4f-b74d-48eb-8993-d857ce626325','B. Da tôi không có cảm giác nhờn','08dd6e4f-b743-46f5-8d84-7e02ea48e398','2025-03-29 06:24:41.311306','2025-03-29 06:24:41.311306'),('08dd6e4f-b74d-492a-84ba-1f1067b827b0','C. Tôi yêu làn da và bản thân mình','08dd6e4f-b743-46f5-8d84-7e02ea48e398','2025-03-29 06:24:41.311306','2025-03-29 06:24:41.311306'),('08dd6e4f-b74d-49a7-8808-a1599688113e','D. Tôi không có mụn hoặc mụn đầu đen','08dd6e4f-b743-46f5-8d84-7e02ea48e398','2025-03-29 06:24:41.311307','2025-03-29 06:24:41.311307');
/*!40000 ALTER TABLE `QuestionOptions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Questions`
--

DROP TABLE IF EXISTS `Questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Questions` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `order_no` int DEFAULT NULL,
  `content` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Questions`
--

LOCK TABLES `Questions` WRITE;
/*!40000 ALTER TABLE `Questions` DISABLE KEYS */;
INSERT INTO `Questions` VALUES ('08dd6e4f-b741-4da4-844e-679be0903eff',1,'Da bạn cảm thấy?','2025-03-29 06:24:41.248587','2025-03-29 06:24:41.248589'),('08dd6e4f-b743-46bc-8b3e-a7a5af025596',2,'Bạn bị mụn và mụn đầu đen?','2025-03-29 06:24:41.248678','2025-03-29 06:24:41.248678'),('08dd6e4f-b743-46e0-8ba0-91ac91aa1b22',3,'Bạn bị mụn?','2025-03-29 06:24:41.248678','2025-03-29 06:24:41.248678'),('08dd6e4f-b743-46ea-89dc-29c5e20489bf',4,'Bạn thích làn da của mình khi nó?','2025-03-29 06:24:41.248678','2025-03-29 06:24:41.248678'),('08dd6e4f-b743-46f5-8d84-7e02ea48e398',5,'Bạn sẽ bớt lo lắng về làn da của mình hơn nếu?','2025-03-29 06:24:41.248679','2025-03-29 06:24:41.248679');
/*!40000 ALTER TABLE `Questions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Schedules`
--

DROP TABLE IF EXISTS `Schedules`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Schedules` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `booking_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `service_detail_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `date` date NOT NULL,
  `reserved_time_start` time NOT NULL,
  `ReservedEndTime` time(6) NOT NULL,
  `status` int NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_Schedules_booking_id` (`booking_id`),
  KEY `IX_Schedules_service_detail_id` (`service_detail_id`),
  CONSTRAINT `FK_Schedules_Bookings_booking_id` FOREIGN KEY (`booking_id`) REFERENCES `Bookings` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Schedules_ServiceDetails_service_detail_id` FOREIGN KEY (`service_detail_id`) REFERENCES `ServiceDetails` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Schedules`
--

LOCK TABLES `Schedules` WRITE;
/*!40000 ALTER TABLE `Schedules` DISABLE KEYS */;
/*!40000 ALTER TABLE `Schedules` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Server`
--

DROP TABLE IF EXISTS `Server`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Server` (
  `Id` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Data` longtext NOT NULL,
  `LastHeartbeat` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Server`
--

LOCK TABLES `Server` WRITE;
/*!40000 ALTER TABLE `Server` DISABLE KEYS */;
INSERT INTO `Server` VALUES ('msi:23540:4a532d43-8dbb-43ec-9d34-5abba79821f4','{\"WorkerCount\":20,\"Queues\":[\"default\"],\"StartedAt\":\"2025-03-29T02:30:46.3246002Z\"}','2025-03-29 03:37:29.575239');
/*!40000 ALTER TABLE `Server` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ServiceCategories`
--

DROP TABLE IF EXISTS `ServiceCategories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ServiceCategories` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `name` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `status` int NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ServiceCategories`
--

LOCK TABLES `ServiceCategories` WRITE;
/*!40000 ALTER TABLE `ServiceCategories` DISABLE KEYS */;
INSERT INTO `ServiceCategories` VALUES ('208fd875-76d8-488c-956d-19f04e83b178','Điều Trị Nám & Tàn Nhang',0,'2025-03-29 06:24:40.865633','2025-03-29 06:24:40.865633'),('237c95b3-67a0-40a8-b153-2aa1f4f7fd87','Xông Hơi & Detox Da',0,'2025-03-29 06:24:40.865636','2025-03-29 06:24:40.865636'),('2854d5b8-c703-492b-924b-b26135e7d372','Thải Độc Da Công Nghệ Cao',0,'2025-03-29 06:24:40.865636','2025-03-29 06:24:40.865636'),('39f33757-015d-48b5-8383-5a09492093a0','Cấp Ẩm & Phục Hồi Da',0,'2025-03-29 06:24:40.865635','2025-03-29 06:24:40.865635'),('3c0f79b0-a2a2-4d72-a642-edff889b74a7','Peel Da & Tái Tạo Da',0,'2025-03-29 06:24:40.865635','2025-03-29 06:24:40.865635'),('49e3eb5d-2b6b-49db-88cd-2c0f537fdf33','Điện Di Vitamin C',0,'2025-03-29 06:24:40.865636','2025-03-29 06:24:40.865636'),('4af573ca-784b-45f1-9645-d09c72ff4a84','Tẩy Tế Bào Chết Chuyên Sâu',0,'2025-03-29 06:24:40.865636','2025-03-29 06:24:40.865636'),('a10b862e-59b7-4a1b-9105-ae0d2cf25007','Chăm Sóc Da Nhạy Cảm',0,'2025-03-29 06:24:40.865634','2025-03-29 06:24:40.865634'),('b2da6c17-82ef-44bf-a33e-dd189670076b','Chăm Sóc Da Chuyên Sâu',0,'2025-03-29 06:24:40.865631','2025-03-29 06:24:40.865631'),('b6fb2873-64ea-4934-841b-a2b02e9801ad','Điều Trị Thâm & Sẹo',0,'2025-03-29 06:24:40.865633','2025-03-29 06:24:40.865633'),('bad9f084-1dd4-481b-9753-1774c9b70135','Trẻ Hóa Da Công Nghệ Cao',0,'2025-03-29 06:24:40.865635','2025-03-29 06:24:40.865635'),('c3d19833-e6e3-4604-b7d9-c0d2e6808cda','Chăm Sóc Da Lão Hóa',0,'2025-03-29 06:24:40.865634','2025-03-29 06:24:40.865634'),('d3f69611-6b85-45da-8020-f28b0c1ec7d6','Chăm Sóc Da Cơ Bản',0,'2025-03-29 06:24:40.865545','2025-03-29 06:24:40.865547'),('d908fd3a-b729-4e9a-9b9a-fd498a20eab6','Cấy Tinh Chất Dưỡng Da',0,'2025-03-29 06:24:40.865635','2025-03-29 06:24:40.865635'),('f277c896-6baa-4e7a-8683-6b90d71a39eb','Điều Trị Mụn',0,'2025-03-29 06:24:40.865633','2025-03-29 06:24:40.865633');
/*!40000 ALTER TABLE `ServiceCategories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ServiceDetails`
--

DROP TABLE IF EXISTS `ServiceDetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ServiceDetails` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `name` varchar(120) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `description` varchar(1024) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `step` int NOT NULL,
  `duration` int NOT NULL,
  `price` decimal(16,2) NOT NULL,
  `day_to_next_step` int NOT NULL,
  `is_deleted` tinyint(1) NOT NULL,
  `service_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_ServiceDetails_service_id` (`service_id`),
  CONSTRAINT `FK_ServiceDetails_Services_service_id` FOREIGN KEY (`service_id`) REFERENCES `Services` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ServiceDetails`
--

LOCK TABLES `ServiceDetails` WRITE;
/*!40000 ALTER TABLE `ServiceDetails` DISABLE KEYS */;
INSERT INTO `ServiceDetails` VALUES ('08dd6e4f-b73a-4d0f-852b-0d80ae8d9763','Làm sạch sâu','Bước làm sạch sâu giúp loại bỏ bụi bẩn, bã nhờn và lớp trang điểm trên da, giúp da thông thoáng và hấp thụ dưỡng chất tốt hơn.',1,30,400000.00,7,0,'08dd6e4f-b71d-4274-87d9-f4c868fc752b','2025-03-29 06:24:41.201365','2025-03-29 06:24:41.201366'),('08dd6e4f-b73c-497c-877c-55fb93ac40bb','Cấp ẩm','Cung cấp độ ẩm chuyên sâu cho làn da bằng các tinh chất dưỡng ẩm cao cấp, giúp da căng mọng và mịn màng hơn.',2,40,800000.00,0,0,'08dd6e4f-b71d-4274-87d9-f4c868fc752b','2025-03-29 06:24:41.201659','2025-03-29 06:24:41.201659'),('08dd6e4f-b73c-49a9-8cc7-2291bd81f1dc','Xông hơi thảo dược','Giúp mở lỗ chân lông và đào thải độc tố trong da.',1,30,500000.00,5,0,'08dd6e4f-b722-4ed0-8e9c-a2dcb1b7fdb4','2025-03-29 06:24:41.201660','2025-03-29 06:24:41.201660'),('08dd6e4f-b73c-49bb-8fa5-fe669b4aeebe','Đắp mặt nạ thải độc','Mặt nạ than hoạt tính giúp hấp thụ dầu thừa và làm sạch sâu lỗ chân lông.',2,40,1000000.00,0,0,'08dd6e4f-b722-4ed0-8e9c-a2dcb1b7fdb4','2025-03-29 06:24:41.201664','2025-03-29 06:24:41.201664'),('08dd6e4f-b73c-49ca-8740-913b302b12bb','Laser trị nám','Sử dụng tia laser phá vỡ sắc tố nám trên da.',1,60,1500000.00,2,0,'08dd6e4f-b722-4f51-8192-db66e5c3e29b','2025-03-29 06:24:41.201664','2025-03-29 06:24:41.201664'),('08dd6e4f-b73c-49d8-8110-7f3203d1b29b','Dưỡng trắng sau laser','Dưỡng da bằng serum làm dịu da và tăng cường độ sáng.',2,40,1000000.00,0,0,'08dd6e4f-b722-4f51-8192-db66e5c3e29b','2025-03-29 06:24:41.201664','2025-03-29 06:24:41.201664'),('08dd6e4f-b73c-49e7-8174-2c0591d432e4','Công nghệ HIFU','Sóng siêu âm hội tụ giúp nâng cơ mặt hiệu quả.',1,60,2000000.00,3,0,'08dd6e4f-b722-4f96-887d-24e601cbb14b','2025-03-29 06:24:41.201664','2025-03-29 06:24:41.201664'),('08dd6e4f-b73c-49f4-882a-8c6f3c675226','Mặt nạ phục hồi','Làm dịu da và kích thích sản sinh collagen.',2,40,1500000.00,0,0,'08dd6e4f-b722-4f96-887d-24e601cbb14b','2025-03-29 06:24:41.201667','2025-03-29 06:24:41.201667'),('08dd6e4f-b73c-4a03-887f-a6ebffedab0e','Xịt oxy tươi','Làm sạch và cung cấp oxy sâu vào da.',1,30,1000000.00,7,0,'08dd6e4f-b722-4fd2-8285-70ae3379bae8','2025-03-29 06:24:41.201667','2025-03-29 06:24:41.201667'),('08dd6e4f-b73c-4a10-8ebe-1e35ec5148a0','Dưỡng ẩm cấp tốc','Sử dụng serum cấp ẩm giúp da căng bóng ngay lập tức.',2,40,1000000.00,0,0,'08dd6e4f-b722-4fd2-8285-70ae3379bae8','2025-03-29 06:24:41.201667','2025-03-29 06:24:41.201667'),('08dd6e4f-b73c-4a1d-8d89-5d012c4bd880','Làm sạch nhẹ nhàng','Sử dụng sữa rửa mặt dịu nhẹ phù hợp với da nhạy cảm.',1,30,800000.00,4,0,'08dd6e4f-b723-4010-8c29-5c2f412ecf50','2025-03-29 06:24:41.201668','2025-03-29 06:24:41.201668'),('08dd6e4f-b73c-4a2c-83b9-0502fe75b343','Dưỡng da phục hồi','Serum chứa thành phần phục hồi giúp da khỏe mạnh hơn.',2,40,800000.00,0,0,'08dd6e4f-b723-4010-8c29-5c2f412ecf50','2025-03-29 06:24:41.201668','2025-03-29 06:24:41.201668'),('08dd6e4f-b73c-4a38-8e8a-db14d58d6cc5','Tẩy trang dịu nhẹ','Loại bỏ lớp trang điểm mà không gây khô da.',1,15,300000.00,2,0,'08dd6e4f-b723-404d-86f7-c93b29cb54e9','2025-03-29 06:24:41.201668','2025-03-29 06:24:41.201668'),('08dd6e4f-b73c-4a48-8acf-e08357dbaaf1','Sữa rửa mặt chuyên dụng','Làm sạch sâu và cung cấp độ ẩm cho da.',2,20,400000.00,3,0,'08dd6e4f-b723-404d-86f7-c93b29cb54e9','2025-03-29 06:24:41.201668','2025-03-29 06:24:41.201668'),('08dd6e4f-b73c-4a55-8d41-ed98df653f28','Tẩy tế bào chết nhẹ nhàng','Loại bỏ da chết, giúp hấp thụ dưỡng chất tốt hơn.',3,15,500000.00,0,0,'08dd6e4f-b723-404d-86f7-c93b29cb54e9','2025-03-29 06:24:41.201669','2025-03-29 06:24:41.201669'),('08dd6e4f-b73c-4a62-88df-21fdfc5b8731','Đắp mặt nạ dưỡng ẩm','Cung cấp nước và làm dịu da khô, nhạy cảm.',1,30,700000.00,4,0,'08dd6e4f-b723-4099-8ddb-b07a3b572a34','2025-03-29 06:24:41.201669','2025-03-29 06:24:41.201670'),('08dd6e4f-b73c-4a71-8bdf-46adba1b7ac2','Tinh chất dưỡng sâu','Serum cấp ẩm giúp da căng bóng tự nhiên.',2,20,800000.00,0,0,'08dd6e4f-b723-4099-8ddb-b07a3b572a34','2025-03-29 06:24:41.201670','2025-03-29 06:24:41.201670'),('08dd6e4f-b73c-4a7e-89d8-8923e6496461','Bổ sung tinh chất collagen','Làm đầy rãnh nhăn và kích thích tái tạo tế bào.',1,40,800000.00,5,0,'08dd6e4f-b723-40d3-8f2a-90700e2bb5c2','2025-03-29 06:24:41.201670','2025-03-29 06:24:41.201670'),('08dd6e4f-b73c-4a8e-86bd-20d476706031','Massage nâng cơ','Giúp săn chắc cơ mặt, giảm chảy xệ.',2,30,600000.00,3,0,'08dd6e4f-b723-40d3-8f2a-90700e2bb5c2','2025-03-29 06:24:41.201670','2025-03-29 06:24:41.201670'),('08dd6e4f-b73c-4a9b-819d-643c3913a12d','Mặt nạ chống lão hóa','Cung cấp dưỡng chất giúp da trẻ hóa.',3,30,600000.00,0,0,'08dd6e4f-b723-40d3-8f2a-90700e2bb5c2','2025-03-29 06:24:41.201670','2025-03-29 06:24:41.201670'),('08dd6e4f-b73c-4aa9-8a41-03ce6958372a','Xông hơi thảo dược','Mở lỗ chân lông, hỗ trợ đào thải độc tố.',1,20,400000.00,2,0,'08dd6e4f-b723-411e-8634-916d998ec66a','2025-03-29 06:24:41.201670','2025-03-29 06:24:41.201671'),('08dd6e4f-b73c-4ab6-87d5-a23e3a37d001','Mặt nạ than hoạt tính','Hấp thụ bã nhờn và giảm nguy cơ mụn đầu đen.',2,30,500000.00,2,0,'08dd6e4f-b723-411e-8634-916d998ec66a','2025-03-29 06:24:41.201671','2025-03-29 06:24:41.201671'),('08dd6e4f-b73c-4ac9-87e4-df115aebbcc1','Serum thải độc da','Cung cấp dưỡng chất giúp làm sạch sâu từ bên trong.',3,30,500000.00,1,0,'08dd6e4f-b723-411e-8634-916d998ec66a','2025-03-29 06:24:41.201671','2025-03-29 06:24:41.201671'),('08dd6e4f-b73c-4ad6-8754-e0a33c3a0406','Dưỡng ẩm kết thúc quy trình','Giữ da mềm mịn và cân bằng độ pH.',4,20,400000.00,0,0,'08dd6e4f-b723-411e-8634-916d998ec66a','2025-03-29 06:24:41.201671','2025-03-29 06:24:41.201671'),('08dd6e4f-b73c-4ae4-8e39-8a8c46d3ed81','Peel da vitamin C','Loại bỏ lớp tế bào chết sạm màu, tái tạo da.',1,30,1200000.00,5,0,'08dd6e4f-b723-4179-8dec-b272219dfa18','2025-03-29 06:24:41.201672','2025-03-29 06:24:41.201672'),('08dd6e4f-b73c-4af2-8395-8d60b9818145','Serum vitamin C','Cải thiện độ sáng da và chống oxy hóa.',2,40,1300000.00,0,0,'08dd6e4f-b723-4179-8dec-b272219dfa18','2025-03-29 06:24:41.201673','2025-03-29 06:24:41.201673'),('08dd6e50-2105-4fdf-81ee-b7c6925991b1','string','string',1,120,0.00,3,0,'6523fa2c-6113-4fdf-b18b-cd375b9f5ded','2025-03-29 06:27:38.522506','2025-03-29 06:27:38.522508');
/*!40000 ALTER TABLE `ServiceDetails` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ServiceImages`
--

DROP TABLE IF EXISTS `ServiceImages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ServiceImages` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `image_url` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `ServiceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_ServiceImages_ServiceId` (`ServiceId`),
  CONSTRAINT `FK_ServiceImages_Services_ServiceId` FOREIGN KEY (`ServiceId`) REFERENCES `Services` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ServiceImages`
--

LOCK TABLES `ServiceImages` WRITE;
/*!40000 ALTER TABLE `ServiceImages` DISABLE KEYS */;
INSERT INTO `ServiceImages` VALUES ('08dd6e4f-b721-4c70-822c-8446aa27239c','https://tatacosmetic.vn/upload/news/1579574574-single_news23-uaruamatlamsachsauhieuqua1.jpg','08dd6e4f-b71d-4274-87d9-f4c868fc752b','2025-03-29 06:24:41.006004','2025-03-29 06:24:41.006005'),('08dd6e4f-b722-4e6f-854f-4ac383e9c02f','https://biocyte.com.vn/wp-content/uploads/2021/07/tai-sao-phai-cap-nuoc-cho-lan-da.png','08dd6e4f-b71d-4274-87d9-f4c868fc752b','2025-03-29 06:24:41.006168','2025-03-29 06:24:41.006168'),('08dd6e4f-b722-4f20-88d6-5cb5976b69a9','https://anmes.vn/wp-content/uploads/2020/07/thao-duoc-cho-xong-hoi-600x343.jpg','08dd6e4f-b722-4ed0-8e9c-a2dcb1b7fdb4','2025-03-29 06:24:41.006299','2025-03-29 06:24:41.006299'),('08dd6e4f-b722-4f3c-8198-193d90b82f07','https://bizweb.dktcdn.net/100/413/259/articles/mat-na-thai-doc.jpg?v=1676022240287','08dd6e4f-b722-4ed0-8e9c-a2dcb1b7fdb4','2025-03-29 06:24:41.006300','2025-03-29 06:24:41.006300'),('08dd6e4f-b722-4f6c-83e0-ef45e3b1c2e8','https://cdn.nhathuoclongchau.com.vn/unsafe/800x0/filters:quality(95)/https://cms-prod.s3-sgn09.fptcloud.com/dieu_tri_nam_da_bang_laser_1_fb9019c433.jpg','08dd6e4f-b722-4f51-8192-db66e5c3e29b','2025-03-29 06:24:41.006348','2025-03-29 06:24:41.006348'),('08dd6e4f-b722-4f7d-8b54-574ad3303675','https://images-1.eucerin.com/~/media/eucerin/local/vn/cham-soc-da-bang-laser/20201109_cach-lam-sach-da-mat-3.jpg?h=366&w=651&la=vi-vn','08dd6e4f-b722-4f51-8192-db66e5c3e29b','2025-03-29 06:24:41.006348','2025-03-29 06:24:41.006348'),('08dd6e4f-b722-4faf-81ee-b54f9b84c904','https://cdn.nhathuoclongchau.com.vn/unsafe/https://cms-prod.s3-sgn09.fptcloud.com/chi_phi_lam_HIFU_0_319e19bea6.jpg','08dd6e4f-b722-4f96-887d-24e601cbb14b','2025-03-29 06:24:41.006394','2025-03-29 06:24:41.006394'),('08dd6e4f-b722-4fc2-88f7-71ae09a54222','https://miri.com.vn/wp-content/uploads/2022/09/mat-na-phuc-hoi-da-la-gi-miri.com_.vn-3-1201x800.jpg','08dd6e4f-b722-4f96-887d-24e601cbb14b','2025-03-29 06:24:41.006395','2025-03-29 06:24:41.006395'),('08dd6e4f-b722-4fe9-8056-5b96a0eb61ab','https://cdn.diemnhangroup.com/s-life/2023/03/phun-oxy-tuoi-co-tac-dung-gi-4.jpg','08dd6e4f-b722-4fd2-8285-70ae3379bae8','2025-03-29 06:24:41.006447','2025-03-29 06:24:41.006447'),('08dd6e4f-b722-4ff9-8d1f-ab0982879bf9','https://thanhhai.com.vn/wp-content/uploads/2022/07/Khi-da-duoc-cap-du-do-am.jpg','08dd6e4f-b722-4fd2-8285-70ae3379bae8','2025-03-29 06:24:41.006447','2025-03-29 06:24:41.006448'),('08dd6e4f-b723-4029-8f97-167dfb646b4a','https://baamboo.com/wp-content/uploads/2021/02/lam-sach-da-mat-1.jpg','08dd6e4f-b723-4010-8c29-5c2f412ecf50','2025-03-29 06:24:41.006496','2025-03-29 06:24:41.006496'),('08dd6e4f-b723-403d-82a9-d65342eb94f8','https://file.hstatic.net/1000006063/file/kem_phuc_hoi_da_giup_tai_tao_lan_da_bi_ton_thuong_7528874626b6473392ddc0cc629f0b9b_grande.jpg','08dd6e4f-b723-4010-8c29-5c2f412ecf50','2025-03-29 06:24:41.006497','2025-03-29 06:24:41.006497'),('08dd6e4f-b723-4064-8ab3-593664fc8eda','https://www.vimaccos.vn/public/upload/nh%20web/cach-su-dung-nuoc-tay-trang-de-co-lan-da-sach-khoe-1.jpg','08dd6e4f-b723-404d-86f7-c93b29cb54e9','2025-03-29 06:24:41.006544','2025-03-29 06:24:41.006544'),('08dd6e4f-b723-4075-8056-33b278e85c19','https://tatacosmetic.vn/upload/detail/2020/01/images/1%20ngay%20dung%20sua%20rua%20mat%20may%20lan_2.jpg','08dd6e4f-b723-404d-86f7-c93b29cb54e9','2025-03-29 06:24:41.006544','2025-03-29 06:24:41.006544'),('08dd6e4f-b723-4089-83e8-ce4b5ad3a0d8','https://bizweb.dktcdn.net/100/239/651/files/02-tay-te-bao-chet-cho-da-mun-la-house.png?v=1596705479645','08dd6e4f-b723-404d-86f7-c93b29cb54e9','2025-03-29 06:24:41.006544','2025-03-29 06:24:41.006544'),('08dd6e4f-b723-40b1-8696-235a4a8cfe08','https://bizweb.dktcdn.net/thumb/1024x1024/100/413/259/files/mat-na-duong-am-tai-nha-15.jpg?v=1676628300013','08dd6e4f-b723-4099-8ddb-b07a3b572a34','2025-03-29 06:24:41.006588','2025-03-29 06:24:41.006588'),('08dd6e4f-b723-40c2-859c-d8d2b0906077','https://megagangnam.com/wp-content/uploads/2022/12/serum-la-gi-01jpg.jpg','08dd6e4f-b723-4099-8ddb-b07a3b572a34','2025-03-29 06:24:41.006588','2025-03-29 06:24:41.006588'),('08dd6e4f-b723-40e9-8399-aa06f76c46aa','https://images2.thanhnien.vn/528068263637045248/2023/5/29/image4-1685327865522406594228.png','08dd6e4f-b723-40d3-8f2a-90700e2bb5c2','2025-03-29 06:24:41.006629','2025-03-29 06:24:41.006629'),('08dd6e4f-b723-40fd-8224-793b54d0b3ce','https://dlccarevn.com/wp-content/uploads/2024/02/cf47fba7e79d36c36f8c.jpeg','08dd6e4f-b723-40d3-8f2a-90700e2bb5c2','2025-03-29 06:24:41.006629','2025-03-29 06:24:41.006629'),('08dd6e4f-b723-410d-84bb-5486a4d7ff99','https://cdn.nhathuoclongchau.com.vn/unsafe/800x0/filters:quality(95)/https://cms-prod.s3-sgn09.fptcloud.com/nhung_loai_mat_na_chong_lao_hoa_tu_thien_nhien_cho_lan_da_min_mang_tuoi_tre_1_98cc222293.jpg','08dd6e4f-b723-40d3-8f2a-90700e2bb5c2','2025-03-29 06:24:41.006630','2025-03-29 06:24:41.006630'),('08dd6e4f-b723-4134-8407-6f298584d381','https://82xbeauty.vn/wp-content/uploads/2022/02/xong-mat-1.jpg','08dd6e4f-b723-411e-8634-916d998ec66a','2025-03-29 06:24:41.006677','2025-03-29 06:24:41.006677'),('08dd6e4f-b723-4148-87c8-2696fc9aa2ee','https://cdn.tgdd.vn//News/1486515//dap-mat-na-than-hoat-tinh-co-tac-dung-gi-top-5-1-845x500.jpg','08dd6e4f-b723-411e-8634-916d998ec66a','2025-03-29 06:24:41.006677','2025-03-29 06:24:41.006677'),('08dd6e4f-b723-4157-8b6e-d1f6610b5f46','https://aladin.com.vn/media/news/2411_serum-duong-da-min.jpg','08dd6e4f-b723-411e-8634-916d998ec66a','2025-03-29 06:24:41.006677','2025-03-29 06:24:41.006677'),('08dd6e4f-b723-4166-8e94-14cde67022bc','https://medlatec.vn/media/13132/content/20201106_cac-buoc-duong-da-2.jpg','08dd6e4f-b723-411e-8634-916d998ec66a','2025-03-29 06:24:41.006677','2025-03-29 06:24:41.006677'),('08dd6e4f-b723-4192-8cb9-0d62e2f9c5fa','https://laskin.vn/wp-content/uploads/2022/09/Sau-khi-peel-da-co-nen-dung-vitamin-C-khong.jpg','08dd6e4f-b723-4179-8dec-b272219dfa18','2025-03-29 06:24:41.006725','2025-03-29 06:24:41.006725'),('08dd6e4f-b723-41a7-86fa-fcd3c0d11631','https://suckhoedoisong.qltns.mediacdn.vn/324455921873985536/2024/1/2/vitamin-c-1704168408870149765090.jpeg','08dd6e4f-b723-4179-8dec-b272219dfa18','2025-03-29 06:24:41.006725','2025-03-29 06:24:41.006725');
/*!40000 ALTER TABLE `ServiceImages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ServiceSkinType`
--

DROP TABLE IF EXISTS `ServiceSkinType`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ServiceSkinType` (
  `ServicesId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `SkinTypesId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`ServicesId`,`SkinTypesId`),
  KEY `IX_ServiceSkinType_SkinTypesId` (`SkinTypesId`),
  CONSTRAINT `FK_ServiceSkinType_Services_ServicesId` FOREIGN KEY (`ServicesId`) REFERENCES `Services` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ServiceSkinType_SkinTypes_SkinTypesId` FOREIGN KEY (`SkinTypesId`) REFERENCES `SkinTypes` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ServiceSkinType`
--

LOCK TABLES `ServiceSkinType` WRITE;
/*!40000 ALTER TABLE `ServiceSkinType` DISABLE KEYS */;
INSERT INTO `ServiceSkinType` VALUES ('08dd6e4f-b722-4ed0-8e9c-a2dcb1b7fdb4','099717de-a077-4324-88c1-9a969450e3e3'),('08dd6e4f-b722-4f96-887d-24e601cbb14b','099717de-a077-4324-88c1-9a969450e3e3'),('08dd6e4f-b722-4fd2-8285-70ae3379bae8','099717de-a077-4324-88c1-9a969450e3e3'),('08dd6e4f-b723-4010-8c29-5c2f412ecf50','099717de-a077-4324-88c1-9a969450e3e3'),('08dd6e4f-b723-4099-8ddb-b07a3b572a34','099717de-a077-4324-88c1-9a969450e3e3'),('08dd6e4f-b723-40d3-8f2a-90700e2bb5c2','099717de-a077-4324-88c1-9a969450e3e3'),('08dd6e4f-b723-411e-8634-916d998ec66a','099717de-a077-4324-88c1-9a969450e3e3'),('08dd6e4f-b723-4179-8dec-b272219dfa18','099717de-a077-4324-88c1-9a969450e3e3'),('08dd6e4f-b722-4ed0-8e9c-a2dcb1b7fdb4','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b722-4f51-8192-db66e5c3e29b','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b722-4f96-887d-24e601cbb14b','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b722-4fd2-8285-70ae3379bae8','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b723-404d-86f7-c93b29cb54e9','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b723-4099-8ddb-b07a3b572a34','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b723-40d3-8f2a-90700e2bb5c2','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b723-4179-8dec-b272219dfa18','2e3d8a91-6594-41e7-b2af-f2892b9a02ce'),('08dd6e4f-b722-4ed0-8e9c-a2dcb1b7fdb4','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b722-4f96-887d-24e601cbb14b','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b722-4fd2-8285-70ae3379bae8','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b723-4010-8c29-5c2f412ecf50','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b723-4099-8ddb-b07a3b572a34','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b723-40d3-8f2a-90700e2bb5c2','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b723-411e-8634-916d998ec66a','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b723-4179-8dec-b272219dfa18','5e223de6-f9c8-4148-8a1b-496bceeffeda'),('08dd6e4f-b722-4ed0-8e9c-a2dcb1b7fdb4','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b722-4f51-8192-db66e5c3e29b','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b722-4f96-887d-24e601cbb14b','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b723-404d-86f7-c93b29cb54e9','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b723-40d3-8f2a-90700e2bb5c2','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b723-411e-8634-916d998ec66a','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b723-4179-8dec-b272219dfa18','b0da5a78-fff0-4065-b2a3-39ab50be102e'),('08dd6e4f-b722-4ed0-8e9c-a2dcb1b7fdb4','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b722-4f51-8192-db66e5c3e29b','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b723-4010-8c29-5c2f412ecf50','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b723-404d-86f7-c93b29cb54e9','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b723-4099-8ddb-b07a3b572a34','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b723-40d3-8f2a-90700e2bb5c2','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b723-411e-8634-916d998ec66a','b2662db8-eb00-491b-b409-94317ad5a8b7'),('08dd6e4f-b723-4179-8dec-b272219dfa18','b2662db8-eb00-491b-b409-94317ad5a8b7');
/*!40000 ALTER TABLE `ServiceSkinType` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Services`
--

DROP TABLE IF EXISTS `Services`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Services` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `service_name` varchar(120) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `service_description` varchar(1024) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `thumbnail_url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `duration` int NOT NULL,
  `price` decimal(16,2) NOT NULL,
  `status` int NOT NULL,
  `service_category_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_Services_service_category_id` (`service_category_id`),
  CONSTRAINT `FK_Services_ServiceCategories_service_category_id` FOREIGN KEY (`service_category_id`) REFERENCES `ServiceCategories` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Services`
--

LOCK TABLES `Services` WRITE;
/*!40000 ALTER TABLE `Services` DISABLE KEYS */;
INSERT INTO `Services` VALUES ('08dd6e4f-b71d-4274-87d9-f4c868fc752b','Chăm sóc da mặt','Dịch vụ chăm sóc da mặt chuyên sâu giúp làm sạch, cấp ẩm và tái tạo da, mang lại làn da khỏe mạnh và tươi trẻ.','https://thanhnien.mediacdn.vn/uploaded/quochung.qc/2018_08_28/MH1/2_RNPT.jpg?width=500',70,2400000.00,0,'d3f69611-6b85-45da-8020-f28b0c1ec7d6','2025-03-29 06:24:41.005283','2025-03-29 06:24:41.005286'),('08dd6e4f-b722-4ed0-8e9c-a2dcb1b7fdb4','Liệu trình Detox da','Phương pháp làm sạch sâu và loại bỏ độc tố giúp làn da khỏe mạnh hơn.','https://skinlab.vn/wp-content/uploads/2024/05/tri-lieu-detox-thanh-loc-da.jpg',70,3000000.00,0,'237c95b3-67a0-40a8-b153-2aa1f4f7fd87','2025-03-29 06:24:41.006234','2025-03-29 06:24:41.006234'),('08dd6e4f-b722-4f51-8192-db66e5c3e29b','Điều trị nám và tàn nhang','Công nghệ cao giúp giảm nám, tàn nhang và làm đều màu da.','https://suckhoedoisong.qltns.mediacdn.vn/324455921873985536/2023/11/7/a555aade67a0b1fee8b1-16993193195541654552642.jpg',100,5000000.00,0,'d3f69611-6b85-45da-8020-f28b0c1ec7d6','2025-03-29 06:24:41.006300','2025-03-29 06:24:41.006300'),('08dd6e4f-b722-4f96-887d-24e601cbb14b','Nâng cơ trẻ hóa da','Dịch vụ sử dụng công nghệ hiện đại giúp nâng cơ và chống lão hóa.','https://vienthammyxuanhuong.com.vn/wp-content/uploads/2023/08/728.jpg',100,7000000.00,0,'c3d19833-e6e3-4604-b7d9-c0d2e6808cda','2025-03-29 06:24:41.006349','2025-03-29 06:24:41.006349'),('08dd6e4f-b722-4fd2-8285-70ae3379bae8','Thải độc da bằng oxy tươi','Cung cấp oxy tinh khiết giúp da khỏe mạnh và tràn đầy sức sống.','https://cdn.nhathuoclongchau.com.vn/unsafe/800x0/https://cms-prod.s3-sgn09.fptcloud.com/cham_soc_da_bang_oxy_tuoi_co_thuc_su_hieu_qua_1_d021ffbfde.jpg',70,4000000.00,0,'b2da6c17-82ef-44bf-a33e-dd189670076b','2025-03-29 06:24:41.006395','2025-03-29 06:24:41.006396'),('08dd6e4f-b723-4010-8c29-5c2f412ecf50','Chăm sóc da nhạy cảm','Dịch vụ đặc biệt dành cho làn da nhạy cảm, giúp phục hồi da.','https://suckhoedoisong.qltns.mediacdn.vn/324455921873985536/2024/11/27/da-nhay-cam-1732696074345442275689.jpg',70,3200000.00,0,'3c0f79b0-a2a2-4d72-a642-edff889b74a7','2025-03-29 06:24:41.006449','2025-03-29 06:24:41.006449'),('08dd6e4f-b723-404d-86f7-c93b29cb54e9','Làm sạch da sâu','Quy trình 3 bước giúp loại bỏ bụi bẩn, dầu thừa, ngăn ngừa mụn.','https://japana.vn/uploads/detail/2021/07/images/bi-quyet-lam-sach-sau-cho-lan-da1.jpeg',50,2400000.00,0,'4af573ca-784b-45f1-9645-d09c72ff4a84','2025-03-29 06:24:41.006497','2025-03-29 06:24:41.006497'),('08dd6e4f-b723-4099-8ddb-b07a3b572a34','Cấp ẩm và phục hồi da','2 bước giúp da mềm mại, đủ ẩm và khỏe mạnh.','https://www.vimed.org/wp-content/uploads/2021/06/serum-cap-am-phuc-hoi-da.jpg',50,3000000.00,0,'39f33757-015d-48b5-8383-5a09492093a0','2025-03-29 06:24:41.006545','2025-03-29 06:24:41.006545'),('08dd6e4f-b723-40d3-8f2a-90700e2bb5c2','Trẻ hóa da với collagen','3 bước giúp tăng cường độ đàn hồi và chống lão hóa.','https://medicskin.vn/wp-content/uploads/2022/12/tre-hoa-da-bang-collagen.png',100,4000000.00,0,'c3d19833-e6e3-4604-b7d9-c0d2e6808cda','2025-03-29 06:24:41.006589','2025-03-29 06:24:41.006589'),('08dd6e4f-b723-411e-8634-916d998ec66a','Thải độc da bằng than hoạt tính','4 bước loại bỏ tạp chất, dầu thừa, giúp da sáng khỏe.','https://misstram.edu.vn/wp-content/uploads/2019/01/thai-doc-da-bang-than-hoat-tinh.jpg',100,3600000.00,0,'4af573ca-784b-45f1-9645-d09c72ff4a84','2025-03-29 06:24:41.006630','2025-03-29 06:24:41.006630'),('08dd6e4f-b723-4179-8dec-b272219dfa18','Làm trắng da với vitamin C','2 bước giúp da đều màu và rạng rỡ hơn.','https://s-cdn.vnluxury.vn/vnlux-media/21/6/17/Vitamin_C.jpg',70,5000000.00,0,'49e3eb5d-2b6b-49db-88cd-2c0f537fdf33','2025-03-29 06:24:41.006682','2025-03-29 06:24:41.006682'),('6523fa2c-6113-4fdf-b18b-cd375b9f5ded','string','string','',120,0.00,0,'a10b862e-59b7-4a1b-9105-ae0d2cf25007','2025-03-29 06:27:38.512168','2025-03-29 06:27:38.512171');
/*!40000 ALTER TABLE `Services` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Set`
--

DROP TABLE IF EXISTS `Set`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Set` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Value` varchar(256) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Score` float NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Set_Key_Value` (`Key`,`Value`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Set`
--

LOCK TABLES `Set` WRITE;
/*!40000 ALTER TABLE `Set` DISABLE KEYS */;
/*!40000 ALTER TABLE `Set` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SkinTypes`
--

DROP TABLE IF EXISTS `SkinTypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SkinTypes` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `name` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `description` varchar(250) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SkinTypes`
--

LOCK TABLES `SkinTypes` WRITE;
/*!40000 ALTER TABLE `SkinTypes` DISABLE KEYS */;
INSERT INTO `SkinTypes` VALUES ('099717de-a077-4324-88c1-9a969450e3e3','Da Thường','Da cân bằng, không quá khô cũng không quá dầu. Lỗ chân lông nhỏ, ít khuyết điểm.','2025-03-29 06:24:41.114288','2025-03-29 06:24:41.114290'),('2e3d8a91-6594-41e7-b2af-f2892b9a02ce','Da Nhạy Cảm','Dễ bị kích ứng, đỏ rát, cần chăm sóc dịu nhẹ và kỹ càng.','2025-03-29 06:24:41.114495','2025-03-29 06:24:41.114495'),('5e223de6-f9c8-4148-8a1b-496bceeffeda','Da Khô','Thiếu độ ẩm, có cảm giác căng, dễ bong tróc và lão hóa sớm.','2025-03-29 06:24:41.114488','2025-03-29 06:24:41.114488'),('b0da5a78-fff0-4065-b2a3-39ab50be102e','Da Hỗn Hợp','Vùng chữ T (trán, mũi, cằm) thường dầu, còn lại khô hoặc bình thường.','2025-03-29 06:24:41.114494','2025-03-29 06:24:41.114494'),('b2662db8-eb00-491b-b409-94317ad5a8b7','Da Dầu','Tuyến bã nhờn hoạt động mạnh, da bóng và dễ nổi mụn.','2025-03-29 06:24:41.114493','2025-03-29 06:24:41.114493');
/*!40000 ALTER TABLE `SkinTypes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `State`
--

DROP TABLE IF EXISTS `State`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `State` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `Name` varchar(20) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Reason` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Data` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_HangFire_State_Job` (`JobId`),
  CONSTRAINT `FK_HangFire_State_Job` FOREIGN KEY (`JobId`) REFERENCES `Job` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `State`
--

LOCK TABLES `State` WRITE;
/*!40000 ALTER TABLE `State` DISABLE KEYS */;
/*!40000 ALTER TABLE `State` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `TherapistCertifications`
--

DROP TABLE IF EXISTS `TherapistCertifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TherapistCertifications` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `therapist_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `file_url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_TherapistCertifications_therapist_id` (`therapist_id`),
  CONSTRAINT `FK_TherapistCertifications_Therapists_therapist_id` FOREIGN KEY (`therapist_id`) REFERENCES `Therapists` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `TherapistCertifications`
--

LOCK TABLES `TherapistCertifications` WRITE;
/*!40000 ALTER TABLE `TherapistCertifications` DISABLE KEYS */;
/*!40000 ALTER TABLE `TherapistCertifications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Therapists`
--

DROP TABLE IF EXISTS `Therapists`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Therapists` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `experience_years` int NOT NULL,
  `bio` varchar(1024) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `status` int NOT NULL,
  `user_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_Therapists_user_id` (`user_id`),
  CONSTRAINT `FK_Therapists_Users_user_id` FOREIGN KEY (`user_id`) REFERENCES `Users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Therapists`
--

LOCK TABLES `Therapists` WRITE;
/*!40000 ALTER TABLE `Therapists` DISABLE KEYS */;
INSERT INTO `Therapists` VALUES ('22359f04-66b6-4b45-8c64-56abaf293c7e',6,'Professional in skin care with holistic therapy techniques.',0,'e3f3943d-a530-4bff-b092-003a30c744e6','2025-03-29 06:24:40.720587','2025-03-29 06:24:40.720587'),('3112afd9-88c0-4a9b-8ac4-229fdabd1f6c',4,'Expert in acne treatment and skin detox therapy.',0,'42f14b61-df2f-4453-b8c0-2a2d50b09679','2025-03-29 06:24:40.720483','2025-03-29 06:24:40.720483'),('37e40e74-5af3-48b5-9add-6ef2b6519718',1,'This is a description field',0,'a10e47d7-e923-48b1-9c2b-fa48c46d4f6d','2025-03-29 06:24:40.697015','2025-03-29 06:24:40.697015'),('49e72b16-71de-421e-b111-e05769bf83fe',5,'This is therapist 2',0,'80293fd8-dc67-4b84-9b4d-d4d85a5afe7e','2025-03-29 06:24:40.719285','2025-03-29 06:24:40.719285'),('9603c5ca-1425-42a0-9224-cc81561551fd',1,'This is Ms. Harley Ferdinand',0,'2383518b-8178-48dc-8569-e004da723c60','2025-03-29 06:24:40.719566','2025-03-29 06:24:40.719566'),('bb0a0359-5c43-4c0d-94c7-6e9dd966dde7',3,'Specialist in skincare and facial therapy with a gentle touch.',0,'b295bbc6-e31d-4f90-975e-fae82442c3da','2025-03-29 06:24:40.719695','2025-03-29 06:24:40.719695'),('bc5e2f4b-0e05-41b4-b3ba-a15fe24e4901',2,'This is Mr. Tommy Vercetti',0,'0b7fc0e8-821d-4018-b119-8cab7c8a1b54','2025-03-29 06:24:40.719631','2025-03-29 06:24:40.719631'),('cc69672e-002a-47dd-9c32-91deb859b872',5,'Certified skin therapist with a passion for rejuvenating skin.',0,'5a64f701-d504-4385-b2ee-dea81d9f5826','2025-03-29 06:24:40.720428','2025-03-29 06:24:40.720428'),('cc7723b5-e88f-46d8-9cdd-fc29096768de',7,'Experienced in anti-aging and lifting facial treatments.',0,'67e498cb-2b36-404d-aed2-50afbddfeb29','2025-03-29 06:24:40.720536','2025-03-29 06:24:40.720536');
/*!40000 ALTER TABLE `Therapists` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Trackings`
--

DROP TABLE IF EXISTS `Trackings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Trackings` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `schedule_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `therapist_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `checkin_time` datetime(6) DEFAULT NULL,
  `checkout_time` datetime(6) DEFAULT NULL,
  `note` varchar(2048) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_Trackings_schedule_id` (`schedule_id`),
  KEY `IX_Trackings_therapist_id` (`therapist_id`),
  CONSTRAINT `FK_Trackings_Schedules_schedule_id` FOREIGN KEY (`schedule_id`) REFERENCES `Schedules` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Trackings_Therapists_therapist_id` FOREIGN KEY (`therapist_id`) REFERENCES `Therapists` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Trackings`
--

LOCK TABLES `Trackings` WRITE;
/*!40000 ALTER TABLE `Trackings` DISABLE KEYS */;
/*!40000 ALTER TABLE `Trackings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Transactions`
--

DROP TABLE IF EXISTS `Transactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Transactions` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `transaction_time` datetime(6) NOT NULL,
  `paydate` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `transaction_value` decimal(16,2) NOT NULL,
  `payment_method` int NOT NULL,
  `transaction_code` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `payment_reference` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `payment_status` int NOT NULL,
  `is_refund_transaction` tinyint(1) NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Transactions`
--

LOCK TABLES `Transactions` WRITE;
/*!40000 ALTER TABLE `Transactions` DISABLE KEYS */;
/*!40000 ALTER TABLE `Transactions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Users`
--

DROP TABLE IF EXISTS `Users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Users` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `fullname` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `username` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `email` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `date_of_birth` date NOT NULL,
  `gender` int NOT NULL,
  `phone_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `role` varchar(25) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `avatar` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `status` int NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Users`
--

LOCK TABLES `Users` WRITE;
/*!40000 ALTER TABLE `Users` DISABLE KEYS */;
INSERT INTO `Users` VALUES ('08372bb8-2275-4365-b0a6-4d8793b60701','Anh Thư','Staff_01','staff01@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1980-12-31',1,'324563447','Staff','',0,'2025-03-29 06:24:40.691121','2025-03-29 06:24:40.691122'),('08dd6e4f-b6e4-49b2-81d2-c8297279b640','Nguyễn Trường Toàn','Admin','vcl@gmail.com','cRSIWKOO8yDkdh8zR8Jv9JcMyb4My5mQ68L6AIgunxE41nMsxA82SjyaFXJEW0Qf0hgAixBzJuI=','0001-01-01',0,'','Admin','',0,'2025-03-29 06:24:40.609972','2025-03-29 06:24:40.609974'),('08dd6e4f-b6ec-4981-8b3e-a696b9b6d17a','Tran Nguyen Quoc Viet','Manager','sample@example.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','0001-01-01',1,'','Manager','',0,'2025-03-29 06:24:40.690449','2025-03-29 06:24:40.690451'),('0b7fc0e8-821d-4018-b119-8cab7c8a1b54','Đức Minh','Therapist_5','example_therapist_2@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1991-11-30',0,'188928777','Therapist','',0,'2025-03-29 06:24:40.719628','2025-03-29 06:24:40.719628'),('0db6c024-81aa-49bb-ad05-449c9b67da99','Lê Thị Hoa','Customer_07','lehoa07@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1999-12-05',1,'0997442829','Customer','',0,'2025-03-29 06:24:40.720754','2025-03-29 06:24:40.720754'),('0f74c828-dd36-4988-8521-ed264157f649','Cao Thị Hạnh','Customer_13','thihanh13@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','2003-02-28',1,'0997442835','Customer','',0,'2025-03-29 06:24:40.720864','2025-03-29 06:24:40.720864'),('102ef617-6273-41aa-bc54-d8cd39220945','Anh Huy','Customer_04','example04@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','2003-08-11',0,'0997442824','Customer','',0,'2025-03-29 06:24:40.720697','2025-03-29 06:24:40.720697'),('2383518b-8178-48dc-8569-e004da723c60','Khánh Linh','Therapist_03','example_therapist_1@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1980-11-30',1,'7429486726','Therapist','',0,'2025-03-29 06:24:40.719557','2025-03-29 06:24:40.719558'),('2c158c51-8007-47b8-844f-adfe91c573ff','Ngô Bảo Châu','Customer_09','ngochau09@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1998-11-23',0,'0997442831','Customer','',0,'2025-03-29 06:24:40.720790','2025-03-29 06:24:40.720790'),('3219157d-e1ae-4c04-8461-d76d06d72984','Lê Thị Thanh','Customer_19','customer19@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1997-09-19',1,'0919191919','Customer','',0,'2025-03-29 06:24:40.720974','2025-03-29 06:24:40.720974'),('42f14b61-df2f-4453-b8c0-2a2d50b09679','Thảo Nhi','Therapist_8','therapist8@example.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1995-01-21',1,'0912888003','Therapist','',0,'2025-03-29 06:24:40.720481','2025-03-29 06:24:40.720481'),('467afd46-4a89-4bf5-8f76-3261b60526d2','Tuấn Kiệt','Customer_01','example01@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','2004-12-02',1,'0194302421','Customer','',0,'2025-03-29 06:24:40.720638','2025-03-29 06:24:40.720638'),('478059a0-c67e-4a74-88a2-e496f1ed6a73','Hữu Phước','Customer_03','example03@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','2003-08-11',0,'0997442823','Customer','',0,'2025-03-29 06:24:40.720680','2025-03-29 06:24:40.720680'),('5a64f701-d504-4385-b2ee-dea81d9f5826','Bảo Khánh','Therapist_7','therapist7@example.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1990-08-05',0,'0912888002','Therapist','',0,'2025-03-29 06:24:40.720422','2025-03-29 06:24:40.720423'),('67e498cb-2b36-404d-aed2-50afbddfeb29','Quang Huy','Therapist_9','therapist9@example.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1988-03-18',0,'0912888004','Therapist','',0,'2025-03-29 06:24:40.720533','2025-03-29 06:24:40.720533'),('67f4e0a4-fbb1-486e-a270-b01d83a24864','Đặng Văn Sơn','Customer_20','customer20@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1996-10-20',0,'0920202020','Customer','',0,'2025-03-29 06:24:40.721191','2025-03-29 06:24:40.721191'),('6842d13d-3f7e-4dc5-82a4-15bc8ec8e473','Đỗ Hoàng Anh','Customer_12','hoanganh12@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1997-05-09',0,'0997442834','Customer','',0,'2025-03-29 06:24:40.720844','2025-03-29 06:24:40.720844'),('7cd14285-3e56-45cc-955b-9de080b257f4','Phạm Thùy Linh','Customer_10','thuylihn10@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','2000-09-17',1,'0997442832','Customer','',0,'2025-03-29 06:24:40.720807','2025-03-29 06:24:40.720807'),('80293fd8-dc67-4b84-9b4d-d4d85a5afe7e','Nguyen Van Lai','Therapist_02','example05@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1997-04-20',0,'8912448336','Therapist','',0,'2025-03-29 06:24:40.719266','2025-03-29 06:24:40.719267'),('93851bc5-dec4-4ab5-8412-682855089ecd','Trần Hoàng Anh','Customer_16','customer16@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','2000-06-16',0,'0916161616','Customer','',0,'2025-03-29 06:24:40.720920','2025-03-29 06:24:40.720921'),('96ce3d21-0eca-4141-a05c-4e7fc6cab1e2','Trần Minh Quân','Customer_06','minhquan06@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','2001-07-15',0,'0997442828','Customer','',0,'2025-03-29 06:24:40.720735','2025-03-29 06:24:40.720735'),('9de6f54c-2b3f-45ca-af36-cb4eba43951e','Lý Thành Đạt','Customer_14','thanhdat14@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1996-10-10',0,'0997442836','Customer','',0,'2025-03-29 06:24:40.720882','2025-03-29 06:24:40.720882'),('a10e47d7-e923-48b1-9c2b-fa48c46d4f6d','Gia Huy','Therapist_1','example04@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1997-04-20',0,'9912448336','Therapist','',0,'2025-03-29 06:24:40.696863','2025-03-29 06:24:40.696864'),('a1370f6b-ece2-4eb9-a741-a34a4c561703','Phạm Bảo Trâm','Customer_17','customer17@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1999-07-17',1,'0917171717','Customer','',0,'2025-03-29 06:24:40.720940','2025-03-29 06:24:40.720940'),('a6255341-0ab0-4a01-bba5-ca684707843f','Bùi Văn Tuấn','Customer_11','vantuant11@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','2001-03-14',0,'0997442833','Customer','',0,'2025-03-29 06:24:40.720826','2025-03-29 06:24:40.720826'),('a8052758-3139-4a87-85db-ebf22f84355a','Nguyễn Minh Đức','Customer_18','customer18@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1998-08-18',0,'0918181818','Customer','',0,'2025-03-29 06:24:40.720957','2025-03-29 06:24:40.720957'),('a98f09e9-9828-41b9-842b-9b87e1ee28aa','Trịnh Minh Khang','Customer_15','minhkhang15@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1995-08-08',0,'0997442837','Customer','',0,'2025-03-29 06:24:40.720899','2025-03-29 06:24:40.720899'),('b295bbc6-e31d-4f90-975e-fae82442c3da','Ngọc Ánh','Therapist_6','therapist6@example.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1992-04-12',1,'0912888001','Therapist','',0,'2025-03-29 06:24:40.719691','2025-03-29 06:24:40.719691'),('c20713bd-53c7-4559-a6cf-6525d7c4765d','Bảo Ngọc','Staff_02','example07@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','2000-05-25',1,'1693837522','Staff','',0,'2025-03-29 06:24:40.696799','2025-03-29 06:24:40.696800'),('c361c758-5dab-42c8-ba5d-550b233ee5ea','Ngọc Sơn','Customer_02','example02@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','2004-12-02',0,'0997442823','Customer','',0,'2025-03-29 06:24:40.720659','2025-03-29 06:24:40.720660'),('e3f3943d-a530-4bff-b092-003a30c744e6','Minh Thư','Therapist_10','therapist10@example.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','1993-07-09',1,'0912888005','Therapist','',0,'2025-03-29 06:24:40.720586','2025-03-29 06:24:40.720586'),('e8f2aaa0-7459-4d19-8f3e-6652cf337112','Nguyễn Huy','Customer_05','example05@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','2003-08-11',0,'0997442827','Customer','',0,'2025-03-29 06:24:40.720714','2025-03-29 06:24:40.720714'),('f17ae057-73b6-42fb-9660-aa01b54ea2fa','Võ Thành Nam','Customer_08','namthanh08@gmail.com','mFABgVWCAmuHzEWg1B18w5oPWp3+UI2kJenvC0lX0Gd6FaRaFKyibx1oEQBLmzgdzAhl3ivuiYI=','2002-06-20',0,'0997442830','Customer','',0,'2025-03-29 06:24:40.720773','2025-03-29 06:24:40.720773');
/*!40000 ALTER TABLE `Users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Vouchers`
--

DROP TABLE IF EXISTS `Vouchers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Vouchers` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `voucher_name` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `voucher_description` varchar(256) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `voucher_code` varchar(256) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `dicount_percentage` decimal(16,2) NOT NULL,
  `start_date` datetime NOT NULL,
  `end_date` datetime NOT NULL,
  `status` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Vouchers`
--

LOCK TABLES `Vouchers` WRITE;
/*!40000 ALTER TABLE `Vouchers` DISABLE KEYS */;
/*!40000 ALTER TABLE `Vouchers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES ('20250328232353_minhtri','8.0.13');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-03-29 18:56:54
