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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AggregatedCounter`
--

LOCK TABLES `AggregatedCounter` WRITE;
/*!40000 ALTER TABLE `AggregatedCounter` DISABLE KEYS */;
INSERT INTO `AggregatedCounter` VALUES (1,'stats:succeeded:2025-03-13',3,'2025-04-13 10:06:17'),(2,'stats:succeeded:2025-03-13-07',1,'2025-03-14 07:50:02'),(3,'stats:succeeded',3,NULL),(4,'stats:succeeded:2025-03-13-08',1,'2025-03-14 08:04:44'),(7,'stats:succeeded:2025-03-13-10',1,'2025-03-14 10:06:17');
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
INSERT INTO `Bookings` VALUES ('35e6e83d-c7d3-45bd-a8f1-620a95d55669','64033f03-9aa1-4a97-a4fb-7ad25020e4ff','aeb59c18-9706-4cf6-9054-d19e05612c7c','08dd61ec-45f2-4936-8dad-1de11863f644',NULL,1200000.000000000000000000000000000000,'2025-03-13 11:02:38.008178',600000.000000000000000000000000000000,1,NULL,'2025-03-13 05:02:38.008165','2025-03-13 05:02:38.008165'),('439a52eb-2734-4d76-bfae-d82bf22635b3','22ec91ef-e556-4271-b9ce-ef739b440326','647bc784-94f6-47e0-9ba1-1891f97c4f28','08dd61ec-45e7-418c-8e3b-30b32a98e50a',NULL,800000.000000000000000000000000000000,'2025-03-18 22:02:38.007269',0.000000000000000000000000000000,0,NULL,'2025-03-13 05:02:38.006809','2025-03-13 05:02:38.006810'),('4fce43f7-370a-4781-b6a0-bc43f91b1835','22ec91ef-e556-4271-b9ce-ef739b440326','c5fb1261-fdda-431d-bda2-ce8dab497349','08dd61ec-45f2-4a4c-8872-08fc659b5aa6',NULL,1500000.000000000000000000000000000000,'2025-03-12 21:02:38.008997',0.000000000000000000000000000000,3,NULL,'2025-03-13 05:02:38.008995','2025-03-13 05:02:38.008995'),('9eca8ed7-9687-418d-aefb-ba694d89922b','82a37717-6447-41c5-8a21-bb86a38ccf27','843f9d1b-e5b3-4bbf-83ab-0d5fd8d56b47','08dd61ec-45f2-49f1-83d9-d1969d9831c0',NULL,500000.000000000000000000000000000000,'2025-03-12 02:02:38.008990',500000.000000000000000000000000000000,2,NULL,'2025-03-13 05:02:38.008980','2025-03-13 05:02:38.008981'),('de8bacd7-0e84-4ba2-9b13-799c17e7a18a','64033f03-9aa1-4a97-a4fb-7ad25020e4ff','647bc784-94f6-47e0-9ba1-1891f97c4f28','08dd61ec-45f2-4aa6-8db6-45f4795a13ee',NULL,900000.000000000000000000000000000000,'2025-03-11 03:02:38.009000',900000.000000000000000000000000000000,2,NULL,'2025-03-13 05:02:38.008998','2025-03-13 05:02:38.008998');
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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb3;
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
  `event_name` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
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
INSERT INTO `Events` VALUES ('08dd61ec-46dc-44e4-8471-a999296e9e79','Hội thảo du lịch nghỉ dưỡng chăm sóc bản thân 2025',100,100000.00,'Đây là nội dung mẫu được đánh máy nhằm mục đích tạo văn bản mẫu','2025-11-20','12:00:00','14:00:00','Hall Alpha','https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQWL6c8Zvkl4lQdlWDTBmrAUzk8WDACENRDRg&s',0,'2025-03-13 05:02:38.545739','2025-03-13 05:02:38.545739'),('08dd61ec-46e4-4f97-8482-f8be7107e53b','Chuyên đề về lợi ích của hệ thống chăm sóc da',250,150000.00,'Đây là nội dung mẫu được đánh máy nhằm mục đích tạo văn bản mẫu','2025-11-22','07:00:00','09:00:00','Hall Alpha','https://file.hstatic.net/200000311493/file/84_99971645a56349b8901748a211a6ca2b_grande.png',0,'2025-03-13 05:02:38.547345','2025-03-13 05:02:38.547345'),('08dd61ec-46e4-4fe1-8ae0-bd8294ecfa91','Triển lãm công nghệ làm đẹp 2025',150,200000.00,'Sự kiện展示 các công nghệ làm đẹp tiên tiến nhất','2025-11-25','09:00:00','12:00:00','Hall Beta','https://nhipcauthuonghieu.vn/wp-content/uploads/2024/07/25715.jpg',0,'2025-03-13 05:02:38.547359','2025-03-13 05:02:38.547359'),('08dd61ec-46e5-4000-85e5-64806d0edcfd','Hội nghị sức khỏe và dinh dưỡng',200,120000.00,'Tìm hiểu về chế độ ăn uống lành mạnh và khoa học','2025-11-28','14:00:00','16:30:00','Hall Gamma','https://i1-suckhoe.vnecdn.net/2022/07/13/young-asian-woman-holding-dumb-1785-8765-1657695811.jpg?w=1020&h=0&q=100&dpr=1&fit=crop&s=QcyeHgcnKb203eW6XGCyNQ',0,'2025-03-13 05:02:38.547361','2025-03-13 05:02:38.547362'),('08dd61ec-46e5-4018-804f-a910b503944d','Workshop yoga và thiền định',80,80000.00,'Trải nghiệm các bài tập thư giãn và cân bằng cơ thể','2025-11-30','06:00:00','08:00:00','Studio Delta','https://balanceyogavilla.com/wp-content/uploads/2024/02/balance-yoga-villa-workshop-hoi-tho-mo-rong-tam-tri-3.jpg',0,'2025-03-13 05:02:38.547364','2025-03-13 05:02:38.547364');
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
INSERT INTO `Feedbacks` VALUES ('08dd61ec-46b7-48e9-8f6f-aa7215f218b8','9eca8ed7-9687-418d-aefb-ba694d89922b',4,5,'Nhân viên rất chuyên nghiệp và thân thiện','Dịch vụ tuyệt vời, da tôi cải thiện rõ rệt','2025-03-13 05:02:38.301247','2025-03-13 05:02:38.301248'),('08dd61ec-46be-41d3-826a-d0bd58c881df','de8bacd7-0e84-4ba2-9b13-799c17e7a18a',5,4,'Kỹ thuật viên rất tận tâm','Dịch vụ tốt nhưng giá hơi cao','2025-03-13 05:02:38.301857','2025-03-13 05:02:38.301857');
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
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Hash`
--

LOCK TABLES `Hash` WRITE;
/*!40000 ALTER TABLE `Hash` DISABLE KEYS */;
INSERT INTO `Hash` VALUES (1,'recurring-job:startup-job','Queue','default',NULL),(2,'recurring-job:startup-job','Cron','0 * * * *',NULL),(3,'recurring-job:startup-job','TimeZoneId','UTC',NULL),(4,'recurring-job:startup-job','Job','{\"Type\":\"System.Console, System.Console, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\",\"Method\":\"WriteLine\",\"ParameterTypes\":\"[\\\"System.String, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e\\\"]\",\"Arguments\":\"[\\\"\\\\\\\"Hangfire job chạy mỗi giờ khi ứng dụng khởi động!\\\\\\\"\\\"]\"}',NULL),(5,'recurring-job:startup-job','CreatedAt','2025-03-13T05:02:40.8116380Z',NULL),(6,'recurring-job:startup-job','NextExecution','2025-03-13T11:00:00.0000000Z',NULL),(7,'recurring-job:startup-job','V','2',NULL),(8,'recurring-job:startup-job','LastExecution','2025-03-13T10:06:01.7251900Z',NULL),(10,'recurring-job:startup-job','LastJobId','3',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Job`
--

LOCK TABLES `Job` WRITE;
/*!40000 ALTER TABLE `Job` DISABLE KEYS */;
INSERT INTO `Job` VALUES (1,3,'Succeeded','{\"Type\":\"System.Console, System.Console, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\",\"Method\":\"WriteLine\",\"ParameterTypes\":\"[\\\"System.String, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e\\\"]\",\"Arguments\":\"[\\\"\\\\\\\"Hangfire job chạy mỗi giờ khi ứng dụng khởi động!\\\\\\\"\\\"]\"}','[\"\\\"Hangfire job chạy mỗi giờ khi ứng dụng khởi động!\\\"\"]','2025-03-13 07:49:47.035040','2025-03-14 07:50:01.559168'),(2,6,'Succeeded','{\"Type\":\"System.Console, System.Console, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\",\"Method\":\"WriteLine\",\"ParameterTypes\":\"[\\\"System.String, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e\\\"]\",\"Arguments\":\"[\\\"\\\\\\\"Hangfire job chạy mỗi giờ khi ứng dụng khởi động!\\\\\\\"\\\"]\"}','[\"\\\"Hangfire job chạy mỗi giờ khi ứng dụng khởi động!\\\"\"]','2025-03-13 08:04:29.098308','2025-03-14 08:04:44.059987'),(3,9,'Succeeded','{\"Type\":\"System.Console, System.Console, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a\",\"Method\":\"WriteLine\",\"ParameterTypes\":\"[\\\"System.String, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e\\\"]\",\"Arguments\":\"[\\\"\\\\\\\"Hangfire job chạy mỗi giờ khi ứng dụng khởi động!\\\\\\\"\\\"]\"}','[\"\\\"Hangfire job chạy mỗi giờ khi ứng dụng khởi động!\\\"\"]','2025-03-13 10:06:01.839950','2025-03-14 10:06:16.813600');
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
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `JobParameter`
--

LOCK TABLES `JobParameter` WRITE;
/*!40000 ALTER TABLE `JobParameter` DISABLE KEYS */;
INSERT INTO `JobParameter` VALUES (1,1,'RecurringJobId','\"startup-job\"'),(2,1,'Time','1741852186'),(3,1,'CurrentCulture','\"vi-VN\"'),(4,1,'CurrentUICulture','\"vi-VN\"'),(5,2,'RecurringJobId','\"startup-job\"'),(6,2,'Time','1741853068'),(7,2,'CurrentCulture','\"vi-VN\"'),(8,2,'CurrentUICulture','\"vi-VN\"'),(9,3,'RecurringJobId','\"startup-job\"'),(10,3,'Time','1741860361'),(11,3,'CurrentCulture','\"\"'),(12,3,'CurrentUICulture','\"\"');
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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
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
  `to_user_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `is_read` tinyint(1) NOT NULL,
  `return_url` varchar(2048) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
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
-- Table structure for table `QuestionOptionSkintypes`
--

DROP TABLE IF EXISTS `QuestionOptionSkintypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `QuestionOptionSkintypes` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `skin_type_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `question_option_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_QuestionOptionSkintypes_question_option_id` (`question_option_id`),
  KEY `IX_QuestionOptionSkintypes_skin_type_id` (`skin_type_id`),
  CONSTRAINT `FK_QuestionOptionSkintypes_QuestionOptions_question_option_id` FOREIGN KEY (`question_option_id`) REFERENCES `QuestionOptions` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_QuestionOptionSkintypes_SkinTypes_skin_type_id` FOREIGN KEY (`skin_type_id`) REFERENCES `SkinTypes` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `QuestionOptionSkintypes`
--

LOCK TABLES `QuestionOptionSkintypes` WRITE;
/*!40000 ALTER TABLE `QuestionOptionSkintypes` DISABLE KEYS */;
INSERT INTO `QuestionOptionSkintypes` VALUES ('08dd61ec-463e-4954-8d55-3ac9cd89c78e','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','08dd61ec-462f-4881-8bd9-3bc0dba36210','2025-03-13 05:02:37.509516','2025-03-13 05:02:37.509516'),('08dd61ec-4641-42eb-861b-5a15978ce697','08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','08dd61ec-462f-4881-8bd9-3bc0dba36210','2025-03-13 05:02:37.509654','2025-03-13 05:02:37.509654'),('08dd61ec-4641-4361-80f4-4f177afeed27','08dd61ec-45b6-40b8-83f8-f8d17e52e73c','08dd61ec-4633-4ddd-803c-de9e8fbd5f7b','2025-03-13 05:02:37.509656','2025-03-13 05:02:37.509656'),('08dd61ec-4641-43e3-8004-524d97edd92f','08dd61ec-45b6-4131-89b1-5cec1d839ce4','08dd61ec-4633-4ddd-803c-de9e8fbd5f7b','2025-03-13 05:02:37.509656','2025-03-13 05:02:37.509656'),('08dd61ec-4641-4410-8d5e-a4df5246d851','08dd61ec-45b6-4131-89b1-5cec1d839ce4','08dd61ec-4633-4e54-8f05-72e69b6bf13f','2025-03-13 05:02:37.509657','2025-03-13 05:02:37.509657'),('08dd61ec-4641-443b-87d8-347d5c434139','08dd61ec-45b6-40b8-83f8-f8d17e52e73c','08dd61ec-4633-4e54-8f05-72e69b6bf13f','2025-03-13 05:02:37.509658','2025-03-13 05:02:37.509658'),('08dd61ec-4641-4463-8f2a-f0a6912bd0b3','08dd61ec-45b6-4157-89f6-a92cf1991447','08dd61ec-4633-4e92-834e-4ded2617da72','2025-03-13 05:02:37.509658','2025-03-13 05:02:37.509658'),('08dd61ec-4641-448a-8a68-9d5dc6a8c8ec','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','08dd61ec-4633-4e92-834e-4ded2617da72','2025-03-13 05:02:37.509659','2025-03-13 05:02:37.509659'),('08dd61ec-4641-44b7-805e-7e417fbf594a','08dd61ec-45b6-40b8-83f8-f8d17e52e73c','08dd61ec-4633-4ece-8097-20014090e3ed','2025-03-13 05:02:37.509659','2025-03-13 05:02:37.509660'),('08dd61ec-4641-44e2-8dd3-e6da8ec9e14e','08dd61ec-45b6-4131-89b1-5cec1d839ce4','08dd61ec-4633-4ece-8097-20014090e3ed','2025-03-13 05:02:37.509660','2025-03-13 05:02:37.509660'),('08dd61ec-4641-450c-8eb3-57579ff78ece','08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','08dd61ec-4633-4f0b-8478-0471e0457975','2025-03-13 05:02:37.509661','2025-03-13 05:02:37.509661'),('08dd61ec-4641-4544-8c80-ccc424b97a28','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','08dd61ec-4633-4f0b-8478-0471e0457975','2025-03-13 05:02:37.509661','2025-03-13 05:02:37.509661'),('08dd61ec-4641-456b-8db7-1ea762ff765b','08dd61ec-45b6-4131-89b1-5cec1d839ce4','08dd61ec-4633-4f42-82e7-db830d1dbb44','2025-03-13 05:02:37.509662','2025-03-13 05:02:37.509662'),('08dd61ec-4641-4592-897f-a58bcf12e6a0','08dd61ec-45b6-40b8-83f8-f8d17e52e73c','08dd61ec-4633-4f42-82e7-db830d1dbb44','2025-03-13 05:02:37.509675','2025-03-13 05:02:37.509676'),('08dd61ec-4641-45c0-8e09-7418fa80aa0b','08dd61ec-45b6-4157-89f6-a92cf1991447','08dd61ec-4633-4f75-8b94-0c90087a786d','2025-03-13 05:02:37.509676','2025-03-13 05:02:37.509676'),('08dd61ec-4641-45ed-8cfe-39533294bc9f','08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','08dd61ec-4633-4f75-8b94-0c90087a786d','2025-03-13 05:02:37.509676','2025-03-13 05:02:37.509676'),('08dd61ec-4641-4652-8787-553b7daf2c49','08dd61ec-45b6-4131-89b1-5cec1d839ce4','08dd61ec-4633-4fab-8416-25c733006bd8','2025-03-13 05:02:37.509676','2025-03-13 05:02:37.509676'),('08dd61ec-4641-46b7-87b0-969b068f6a73','08dd61ec-45b6-40b8-83f8-f8d17e52e73c','08dd61ec-4633-4fab-8416-25c733006bd8','2025-03-13 05:02:37.509676','2025-03-13 05:02:37.509676'),('08dd61ec-4641-4717-8151-bedc5762afc8','08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','08dd61ec-4633-4fe6-813d-0be3680bbcd5','2025-03-13 05:02:37.509677','2025-03-13 05:02:37.509677'),('08dd61ec-4641-473e-8059-041fbc528fde','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','08dd61ec-4633-4fe6-813d-0be3680bbcd5','2025-03-13 05:02:37.509677','2025-03-13 05:02:37.509677'),('08dd61ec-4641-4762-8fc2-3ab2e7d61069','08dd61ec-45b6-4157-89f6-a92cf1991447','08dd61ec-4634-4019-8f2e-6d48882b5ef3','2025-03-13 05:02:37.509677','2025-03-13 05:02:37.509677'),('08dd61ec-4641-4787-89de-0444e5020352','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','08dd61ec-4634-4019-8f2e-6d48882b5ef3','2025-03-13 05:02:37.509677','2025-03-13 05:02:37.509677'),('08dd61ec-4641-47ab-888b-0718632da21d','08dd61ec-45b6-40b8-83f8-f8d17e52e73c','08dd61ec-4634-404b-8be6-7db51bd0ce9a','2025-03-13 05:02:37.509678','2025-03-13 05:02:37.509678'),('08dd61ec-4641-47d8-8152-bf83b6d1715a','08dd61ec-45b6-4131-89b1-5cec1d839ce4','08dd61ec-4634-404b-8be6-7db51bd0ce9a','2025-03-13 05:02:37.509678','2025-03-13 05:02:37.509678'),('08dd61ec-4641-4805-8821-8d521f07087c','08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','08dd61ec-4634-4081-8ae6-b33107d4dc28','2025-03-13 05:02:37.509678','2025-03-13 05:02:37.509678'),('08dd61ec-4641-489e-8c0b-64105bd629e3','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','08dd61ec-4634-4081-8ae6-b33107d4dc28','2025-03-13 05:02:37.509678','2025-03-13 05:02:37.509678'),('08dd61ec-4641-4916-8bb6-72e5f690e145','08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','08dd61ec-4634-40b5-87dd-584421fc9ab0','2025-03-13 05:02:37.509678','2025-03-13 05:02:37.509678'),('08dd61ec-4641-498f-849c-c23d326e0f3a','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','08dd61ec-4634-40b5-87dd-584421fc9ab0','2025-03-13 05:02:37.509679','2025-03-13 05:02:37.509679'),('08dd61ec-4641-4a12-8b15-bb967c777b03','08dd61ec-45b6-4157-89f6-a92cf1991447','08dd61ec-4634-40e6-8ee0-55ceabdf50ec','2025-03-13 05:02:37.509681','2025-03-13 05:02:37.509681'),('08dd61ec-4641-4a94-8713-6c265eabacf6','08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','08dd61ec-4634-40e6-8ee0-55ceabdf50ec','2025-03-13 05:02:37.509681','2025-03-13 05:02:37.509681'),('08dd61ec-4641-4b0a-8579-b3deea0eafee','08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','08dd61ec-4634-4119-8f9a-d9b01af3bffd','2025-03-13 05:02:37.509681','2025-03-13 05:02:37.509681'),('08dd61ec-4641-4b8b-8d59-55d68ef47ba9','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','08dd61ec-4634-4119-8f9a-d9b01af3bffd','2025-03-13 05:02:37.509682','2025-03-13 05:02:37.509682'),('08dd61ec-4641-4c04-86fc-d2b86f83e072','08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','08dd61ec-4634-414d-80a0-48a8e07b044d','2025-03-13 05:02:37.509682','2025-03-13 05:02:37.509682'),('08dd61ec-4641-4c7a-804e-c03145ee8b5d','08dd61ec-45b6-4157-89f6-a92cf1991447','08dd61ec-4634-414d-80a0-48a8e07b044d','2025-03-13 05:02:37.509682','2025-03-13 05:02:37.509682'),('08dd61ec-4641-4d03-87ee-a596390396a7','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','08dd61ec-4634-417f-8426-0cfdad3d4f64','2025-03-13 05:02:37.509682','2025-03-13 05:02:37.509682'),('08dd61ec-4641-4d77-8838-add314921dca','08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','08dd61ec-4634-417f-8426-0cfdad3d4f64','2025-03-13 05:02:37.509683','2025-03-13 05:02:37.509683'),('08dd61ec-4641-4dfc-8a00-bf5359a5adaa','08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','08dd61ec-4634-41ec-8d46-a8662caa8528','2025-03-13 05:02:37.509683','2025-03-13 05:02:37.509683'),('08dd61ec-4641-4e4d-891e-85c530a3ccf9','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','08dd61ec-4634-41ec-8d46-a8662caa8528','2025-03-13 05:02:37.509683','2025-03-13 05:02:37.509683'),('08dd61ec-4641-4e7d-8d4c-b364d10b354a','08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','08dd61ec-4634-4223-8990-4f485a6b5ab6','2025-03-13 05:02:37.509683','2025-03-13 05:02:37.509683'),('08dd61ec-4641-4ea7-8911-9f3adff665cd','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','08dd61ec-4634-4223-8990-4f485a6b5ab6','2025-03-13 05:02:37.509683','2025-03-13 05:02:37.509683');
/*!40000 ALTER TABLE `QuestionOptionSkintypes` ENABLE KEYS */;
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
INSERT INTO `QuestionOptions` VALUES ('08dd61ec-462f-4881-8bd9-3bc0dba36210','A. Khô, bình tĩnh và dễ chăm sóc','08dd61ec-461b-42dd-8503-ae7be0fb4d6e','2025-03-13 05:02:37.411741','2025-03-13 05:02:37.411741'),('08dd61ec-4633-4ddd-803c-de9e8fbd5f7b','B. Bóng, nhờn và có một chút vấn đề','08dd61ec-461b-42dd-8503-ae7be0fb4d6e','2025-03-13 05:02:37.411911','2025-03-13 05:02:37.411911'),('08dd61ec-4633-4e54-8f05-72e69b6bf13f','C. Vùng trán và mũi của tôi trơn','08dd61ec-461b-42dd-8503-ae7be0fb4d6e','2025-03-13 05:02:37.411912','2025-03-13 05:02:37.411912'),('08dd61ec-4633-4e92-834e-4ded2617da72','D. Căng sau khi tôi rửa bằng chất tẩy rửa không tự nhiên','08dd61ec-461b-42dd-8503-ae7be0fb4d6e','2025-03-13 05:02:37.411912','2025-03-13 05:02:37.411912'),('08dd61ec-4633-4ece-8097-20014090e3ed','A. Luôn luôn','08dd61ec-4621-4514-8f6f-e5fc8ae2e744','2025-03-13 05:02:37.411912','2025-03-13 05:02:37.411912'),('08dd61ec-4633-4f0b-8478-0471e0457975','B. Rất hiếm khi','08dd61ec-4621-4514-8f6f-e5fc8ae2e744','2025-03-13 05:02:37.411912','2025-03-13 05:02:37.411912'),('08dd61ec-4633-4f42-82e7-db830d1dbb44','C. Vào thời điểm kinh nguyệt của tôi','08dd61ec-4621-4514-8f6f-e5fc8ae2e744','2025-03-13 05:02:37.411912','2025-03-13 05:02:37.411912'),('08dd61ec-4633-4f75-8b94-0c90087a786d','D. Thỉnh thoảng','08dd61ec-4621-4514-8f6f-e5fc8ae2e744','2025-03-13 05:02:37.411913','2025-03-13 05:02:37.411913'),('08dd61ec-4633-4fab-8416-25c733006bd8','A. Trên trán, dọc theo đường chân tóc và trên cằm','08dd61ec-4621-4591-8ae7-7f838e1bb389','2025-03-13 05:02:37.411913','2025-03-13 05:02:37.411913'),('08dd61ec-4633-4fe6-813d-0be3680bbcd5','B. Rất hiếm khi','08dd61ec-4621-4591-8ae7-7f838e1bb389','2025-03-13 05:02:37.411913','2025-03-13 05:02:37.411913'),('08dd61ec-4634-4019-8f2e-6d48882b5ef3','C. Thường là khi tôi không rửa mặt bằng chất tẩy rửa tự nhiên','08dd61ec-4621-4591-8ae7-7f838e1bb389','2025-03-13 05:02:37.411913','2025-03-13 05:02:37.411913'),('08dd61ec-4634-404b-8be6-7db51bd0ce9a','D. Một lần một tháng','08dd61ec-4621-4591-8ae7-7f838e1bb389','2025-03-13 05:02:37.411913','2025-03-13 05:02:37.411913'),('08dd61ec-4634-4081-8ae6-b33107d4dc28','A. Không có mụn','08dd61ec-4621-45c9-858e-5dd6648df405','2025-03-13 05:02:37.411914','2025-03-13 05:02:37.411914'),('08dd61ec-4634-40b5-87dd-584421fc9ab0','B. Cảm thấy sạch','08dd61ec-4621-45c9-858e-5dd6648df405','2025-03-13 05:02:37.411914','2025-03-13 05:02:37.411914'),('08dd61ec-4634-40e6-8ee0-55ceabdf50ec','C. Không đỏ và viêm','08dd61ec-4621-45c9-858e-5dd6648df405','2025-03-13 05:02:37.411914','2025-03-13 05:02:37.411914'),('08dd61ec-4634-4119-8f9a-d9b01af3bffd','D. Trông khỏe mạnh','08dd61ec-4621-45c9-858e-5dd6648df405','2025-03-13 05:02:37.411914','2025-03-13 05:02:37.411914'),('08dd61ec-4634-414d-80a0-48a8e07b044d','A. Tôi sử dụng các sản phẩm chăm sóc da tự nhiên hai lần một ngày','08dd61ec-4621-45fc-8d23-5397d46227b7','2025-03-13 05:02:37.411914','2025-03-13 05:02:37.411914'),('08dd61ec-4634-417f-8426-0cfdad3d4f64','B. Da tôi không có cảm giác nhờn','08dd61ec-4621-45fc-8d23-5397d46227b7','2025-03-13 05:02:37.411914','2025-03-13 05:02:37.411915'),('08dd61ec-4634-41ec-8d46-a8662caa8528','C. Tôi yêu làn da và bản thân mình','08dd61ec-4621-45fc-8d23-5397d46227b7','2025-03-13 05:02:37.411915','2025-03-13 05:02:37.411915'),('08dd61ec-4634-4223-8990-4f485a6b5ab6','D. Tôi không có mụn hoặc mụn đầu đen','08dd61ec-4621-45fc-8d23-5397d46227b7','2025-03-13 05:02:37.411915','2025-03-13 05:02:37.411915');
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
INSERT INTO `Questions` VALUES ('08dd61ec-461b-42dd-8503-ae7be0fb4d6e',1,'Da bạn cảm thấy?','2025-03-13 05:02:37.280663','2025-03-13 05:02:37.280663'),('08dd61ec-4621-4514-8f6f-e5fc8ae2e744',2,'Bạn bị mụn và mụn đầu đen?','2025-03-13 05:02:37.280924','2025-03-13 05:02:37.280924'),('08dd61ec-4621-4591-8ae7-7f838e1bb389',3,'Bạn bị mụn?','2025-03-13 05:02:37.280924','2025-03-13 05:02:37.280924'),('08dd61ec-4621-45c9-858e-5dd6648df405',4,'Bạn thích làn da của mình khi nó?','2025-03-13 05:02:37.280925','2025-03-13 05:02:37.280925'),('08dd61ec-4621-45fc-8d23-5397d46227b7',5,'Bạn sẽ bớt lo lắng về làn da của mình hơn nếu?','2025-03-13 05:02:37.280925','2025-03-13 05:02:37.280925');
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
INSERT INTO `Server` VALUES ('aa34c7fcc898:1005:a3b9a0ba-e38a-4fb5-8b0d-2f0cd527c542','{\"WorkerCount\":20,\"Queues\":[\"default\"],\"StartedAt\":\"2025-03-13T10:06:43.115925Z\"}','2025-03-13 10:06:43.124627'),('aa34c7fcc898:857:0e904b69-e9fd-4e8d-a48d-e9b03f9635d1','{\"WorkerCount\":20,\"Queues\":[\"default\"],\"StartedAt\":\"2025-03-13T10:06:01.2364533Z\"}','2025-03-13 10:06:01.244163');
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
INSERT INTO `ServiceCategories` VALUES ('15caac33-5711-4db9-afa5-6e1a16faf3c5','Xông Hơi',0,'2025-03-13 05:02:36.466813','2025-03-13 05:02:36.466813'),('240b8aba-6821-4b88-962d-b5165c45e1bf','Chăm Sóc Da Mặt',0,'2025-03-13 05:02:36.466801','2025-03-13 05:02:36.466802'),('2987eacd-2b20-4876-b761-00dfe37f6b43','Massage Kỹ Thuật Cao',0,'2025-03-13 05:02:36.466387','2025-03-13 05:02:36.466388'),('613e40c5-eb23-4543-a0d7-7f00937dad5f','Dịch Vụ Tẩy Lông',0,'2025-03-13 05:02:36.466812','2025-03-13 05:02:36.466812'),('94774792-85a2-4c62-9770-d1d407c733e0','Chăm Sóc Cơ Thể',0,'2025-03-13 05:02:36.466810','2025-03-13 05:02:36.466810');
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
  `name` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
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
INSERT INTO `ServiceDetails` VALUES ('08dd61ec-46c8-450c-89fa-0f23ec381384','Làm sạch da','Loại bỏ bụi bẩn và dầu thừa.',1,30,300000.00,5,0,'08dd61ec-45e7-418c-8e3b-30b32a98e50a','2025-03-13 05:02:38.413419','2025-03-13 05:02:38.413420'),('08dd61ec-46ce-40ef-83fd-ab605283e230','Điều trị mụn','Sử dụng công nghệ trị mụn.',1,60,800000.00,7,0,'08dd61ec-45f2-4936-8dad-1de11863f644','2025-03-13 05:02:38.413917','2025-03-13 05:02:38.413917'),('08dd61ec-46ce-41a9-8638-e4ec7c24bc52','Dưỡng trắng','Cải thiện độ sáng da.',1,45,900000.00,10,0,'08dd61ec-45f2-49f1-83d9-d1969d9831c0','2025-03-13 05:02:38.413921','2025-03-13 05:02:38.413921'),('08dd61ec-46ce-4210-8fd8-c6289f35dd65','Trẻ hóa','Kích thích collagen.',1,60,1200000.00,8,0,'08dd61ec-45f2-4a4c-8872-08fc659b5aa6','2025-03-13 05:02:38.413922','2025-03-13 05:02:38.413922'),('08dd61ec-46ce-423a-8c53-7378348d8c7c','Tẩy tế bào','Làm sạch sâu.',1,20,200000.00,3,0,'08dd61ec-45f2-4aa6-8db6-45f4795a13ee','2025-03-13 05:02:38.413922','2025-03-13 05:02:38.413922'),('08dd61ec-46ce-427b-836c-e5d3fe225d15','Massage','Thư giãn cơ mặt.',1,30,350000.00,5,0,'08dd61ec-45f2-4b09-8092-23fff54e6247','2025-03-13 05:02:38.413923','2025-03-13 05:02:38.413923'),('08dd61ec-46ce-42b0-8989-fc620acab2a0','Chăm sóc sâu','Đắp mặt nạ cao cấp.',1,60,1500000.00,12,0,'08dd61ec-45f2-4b72-82b4-4fcd20edeeab','2025-03-13 05:02:38.413929','2025-03-13 05:02:38.413929'),('08dd61ec-46ce-4301-81f1-72ed6b0f28ac','Trị nám','Giảm sắc tố.',1,45,1000000.00,7,0,'08dd61ec-45f2-4bc7-8118-051e4702f8ee','2025-03-13 05:02:38.413930','2025-03-13 05:02:38.413930'),('08dd61ec-46ce-432a-8d09-ce99007e6d3c','Dưỡng chất','Cung cấp vitamin.',1,40,500000.00,6,0,'08dd61ec-45f2-4c1d-8039-b407841dcf74','2025-03-13 05:02:38.413930','2025-03-13 05:02:38.413930'),('08dd61ec-46ce-4366-89f1-820190cd3fb6','Chăm sóc mắt','Giảm quầng thâm.',1,30,400000.00,4,0,'08dd61ec-45f2-4c73-8728-c43c5c0cd10e','2025-03-13 05:02:38.413931','2025-03-13 05:02:38.413931');
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
/*!40000 ALTER TABLE `ServiceImages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ServiceRecommendation`
--

DROP TABLE IF EXISTS `ServiceRecommendation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ServiceRecommendation` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `service_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `skin_id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_ServiceRecommendation_service_id` (`service_id`),
  KEY `IX_ServiceRecommendation_skin_id` (`skin_id`),
  CONSTRAINT `FK_ServiceRecommendation_Services_service_id` FOREIGN KEY (`service_id`) REFERENCES `Services` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ServiceRecommendation_SkinTypes_skin_id` FOREIGN KEY (`skin_id`) REFERENCES `SkinTypes` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ServiceRecommendation`
--

LOCK TABLES `ServiceRecommendation` WRITE;
/*!40000 ALTER TABLE `ServiceRecommendation` DISABLE KEYS */;
INSERT INTO `ServiceRecommendation` VALUES ('08dd61ec-460f-4191-832c-098024121127','08dd61ec-45e7-418c-8e3b-30b32a98e50a','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','2025-03-13 05:02:37.195544','2025-03-13 05:02:37.195544'),('08dd61ec-4614-433e-8109-4e52945d36e7','08dd61ec-45f2-4936-8dad-1de11863f644','08dd61ec-45b6-40b8-83f8-f8d17e52e73c','2025-03-13 05:02:37.195886','2025-03-13 05:02:37.195886'),('08dd61ec-4614-43cf-8af6-a968e629c6b3','08dd61ec-45f2-49f1-83d9-d1969d9831c0','08dd61ec-45b6-4131-89b1-5cec1d839ce4','2025-03-13 05:02:37.195887','2025-03-13 05:02:37.195887'),('08dd61ec-4614-4435-8dba-4f2e26d47028','08dd61ec-45f2-4a4c-8872-08fc659b5aa6','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','2025-03-13 05:02:37.195888','2025-03-13 05:02:37.195888'),('08dd61ec-4614-4489-843a-d12180fb1b2b','08dd61ec-45f2-4aa6-8db6-45f4795a13ee','08dd61ec-45b6-40b8-83f8-f8d17e52e73c','2025-03-13 05:02:37.195888','2025-03-13 05:02:37.195888'),('08dd61ec-4614-44dd-83b4-5dadb5c1c8b8','08dd61ec-45f2-4b09-8092-23fff54e6247','08dd61ec-45b6-4131-89b1-5cec1d839ce4','2025-03-13 05:02:37.195889','2025-03-13 05:02:37.195889'),('08dd61ec-4614-4528-83df-369214c5926a','08dd61ec-45f2-4b72-82b4-4fcd20edeeab','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','2025-03-13 05:02:37.195905','2025-03-13 05:02:37.195905'),('08dd61ec-4614-4584-80a2-68609a4dc9e9','08dd61ec-45f2-4bc7-8118-051e4702f8ee','08dd61ec-45b6-40b8-83f8-f8d17e52e73c','2025-03-13 05:02:37.195905','2025-03-13 05:02:37.195905'),('08dd61ec-4614-4601-8477-5e5192656203','08dd61ec-45f2-4c1d-8039-b407841dcf74','08dd61ec-45b6-4131-89b1-5cec1d839ce4','2025-03-13 05:02:37.195906','2025-03-13 05:02:37.195906'),('08dd61ec-4614-4670-8a62-9320a815a53b','08dd61ec-45f2-4c73-8728-c43c5c0cd10e','08dd61ec-45b0-45d0-8201-ad42c3c74ba1','2025-03-13 05:02:37.195913','2025-03-13 05:02:37.195913');
/*!40000 ALTER TABLE `ServiceRecommendation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Services`
--

DROP TABLE IF EXISTS `Services`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Services` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `service_name` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
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
INSERT INTO `Services` VALUES ('08dd61ec-45e7-418c-8e3b-30b32a98e50a','Dịch vụ dưỡng ẩm sâu','Cung cấp dưỡng chất và khóa ẩm lâu dài cho da khô.','https://easysalon.vn/wp-content/uploads/2021/04/bang-gia-dich-vu-Spa-2.jpg',1,1100000.00,0,'15caac33-5711-4db9-afa5-6e1a16faf3c5','2025-03-13 05:02:36.938017','2025-03-13 05:02:36.938017'),('08dd61ec-45f2-4936-8dad-1de11863f644','Dịch vụ tái tạo da với mặt nạ collagen','Giảm bong tróc và tăng độ đàn hồi.','https://cdn.dealtoday.vn/img/s800x400/0341ba775a40443d85e685ef19c790ab.jpg?sign=OVhemMvb6L5yOXMFOsSjxw',1,2000000.00,0,'15caac33-5711-4db9-afa5-6e1a16faf3c5','2025-03-13 05:02:36.938610','2025-03-13 05:02:36.938610'),('08dd61ec-45f2-49f1-83d9-d1969d9831c0','Dịch vụ làm sạch nhẹ nhàng','Loại bỏ bụi bẩn mà không làm khô da.','https://cdn.dealtoday.vn/img/c280x280/LBelle-Beauty-Lay-nhan-mun-va-dien-di-lanh-phuc-hoi-avt_17092024160739.jpg?sign=cC_ZykqWSGqY2KOLsTW8IQ',1,1400000.00,0,'15caac33-5711-4db9-afa5-6e1a16faf3c5','2025-03-13 05:02:36.938612','2025-03-13 05:02:36.938612'),('08dd61ec-45f2-4a4c-8872-08fc659b5aa6','Dịch vụ trẻ hóa với vitamin E','Cải thiện độ căng bóng và chống oxy hóa.','https://benhvienthammynaman.com/wp-content/uploads/2023/06/truyen-vitamin-tre-hoa-da-mat-1.jpg',1,2700000.00,0,'15caac33-5711-4db9-afa5-6e1a16faf3c5','2025-03-13 05:02:36.938612','2025-03-13 05:02:36.938612'),('08dd61ec-45f2-4aa6-8db6-45f4795a13ee','Dịch vụ kiểm soát nhờn','Giảm tiết dầu và se khít lỗ chân lông.','https://o2skin.vn/wp-content/uploads/2024/05/hinh-anh-gioi-thieu-dich-vu-mat-na-dieu-tri-mun-va-kiem-soat-nhon-3.png',1,1100000.00,0,'240b8aba-6821-4b88-962d-b5165c45e1bf','2025-03-13 05:02:36.938613','2025-03-13 05:02:36.938613'),('08dd61ec-45f2-4b09-8092-23fff54e6247','Dịch vụ trị mụn chuyên sâu','Giảm viêm và ngăn ngừa mụn tái phát.','https://anabelspa.vn/wp-content/uploads/2023/06/dieu-tri-mun-chuyen-sau-500x467.jpg',1,1550000.00,0,'240b8aba-6821-4b88-962d-b5165c45e1bf','2025-03-13 05:02:36.938614','2025-03-13 05:02:36.938614'),('08dd61ec-45f2-4b72-82b4-4fcd20edeeab','Dịch vụ tẩy tế bào chết hóa học','Loại bỏ tế bào chết và kiểm soát dầu thừa.','https://www.elle.vn/app/uploads/2020/08/13/411653/da-dau-1.jpg',1,2300000.00,0,'240b8aba-6821-4b88-962d-b5165c45e1bf','2025-03-13 05:02:36.938615','2025-03-13 05:02:36.938615'),('08dd61ec-45f2-4bc7-8118-051e4702f8ee','Dịch vụ làm sạch sâu với than hoạt tính','Hút nhờn và giảm mụn đầu đen.','https://haianhspa.com.vn/wp-content/uploads/2020/03/IMG_8937-1536x1024.jpg',1,2000000.00,0,'240b8aba-6821-4b88-962d-b5165c45e1bf','2025-03-13 05:02:36.938615','2025-03-13 05:02:36.938615'),('08dd61ec-45f2-4c1d-8039-b407841dcf74','Dịch vụ cân bằng độ ẩm vùng chữ T','Giảm nhờn vùng trán và dưỡng ẩm vùng má.','https://bloganchoi.com/wp-content/uploads/2016/09/vung-chu-t-la-gi.jpg',1,1500000.00,0,'2987eacd-2b20-4876-b761-00dfe37f6b43','2025-03-13 05:02:36.938622','2025-03-13 05:02:36.938622'),('08dd61ec-45f2-4c73-8728-c43c5c0cd10e','Dịch vụ trị mụn cục bộ','Đặc trị mụn ở vùng trán và cằm.','https://blissbeauty.vn/wp-content/uploads/2023/03/7.png',1,1600000.00,0,'2987eacd-2b20-4876-b761-00dfe37f6b43','2025-03-13 05:02:36.938623','2025-03-13 05:02:36.938623'),('08dd61ec-45f2-4cc8-8ab3-71c22b47d0e9','Dịch vụ làm sạch 2 bước','Làm sạch dầu vùng chữ T và giữ ẩm vùng má.','https://linhtranspa.com/wp-content/uploads/2021/05/dich-vu-cham-soc-da-mat-chuyen-sau-2.jpg',1,800000.00,0,'2987eacd-2b20-4876-b761-00dfe37f6b43','2025-03-13 05:02:36.938624','2025-03-13 05:02:36.938624'),('08dd61ec-45f2-4d1a-8a81-ea9c8e712401','Dịch vụ tẩy da chết enzyme','Loại bỏ tế bào chết nhẹ nhàng và không gây khô da.','https://thammymisstram.vn/wp-content/uploads/2021/08/tay-da-chet-bang-enzyme.jpg',1,900000.00,0,'2987eacd-2b20-4876-b761-00dfe37f6b43','2025-03-13 05:02:36.938624','2025-03-13 05:02:36.938624');
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Set`
--

LOCK TABLES `Set` WRITE;
/*!40000 ALTER TABLE `Set` DISABLE KEYS */;
INSERT INTO `Set` VALUES (1,'recurring-jobs','startup-job',1741860000,NULL);
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
INSERT INTO `SkinTypes` VALUES ('08dd61ec-45b0-45d0-8201-ad42c3c74ba1','Da khô','Da khô có tuyến bã nhờn hoạt động kém, dễ bị bong tróc và thiếu ẩm...','2025-03-13 05:02:36.578709','2025-03-13 05:02:36.578709'),('08dd61ec-45b6-40b8-83f8-f8d17e52e73c','Da dầu','Da dầu có tuyến bã nhờn hoạt động mạnh, dễ bị bóng nhờn và nổi mụn...','2025-03-13 05:02:36.579037','2025-03-13 05:02:36.579037'),('08dd61ec-45b6-4131-89b1-5cec1d839ce4','Da hỗn hợp','Da hỗn hợp có vùng chữ T dầu và các vùng khác khô hoặc thường...','2025-03-13 05:02:36.579045','2025-03-13 05:02:36.579045'),('08dd61ec-45b6-4157-89f6-a92cf1991447','Da nhạy cảm','Da nhạy cảm dễ bị kích ứng với các yếu tố môi trường và mỹ phẩm...','2025-03-13 05:02:36.579045','2025-03-13 05:02:36.579045'),('08dd61ec-45b6-417a-8d1c-ffd26bbc9c0c','Da thường','Da thường cân bằng giữa dầu và độ ẩm, ít gặp vấn đề...','2025-03-13 05:02:36.579045','2025-03-13 05:02:36.579045');
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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `State`
--

LOCK TABLES `State` WRITE;
/*!40000 ALTER TABLE `State` DISABLE KEYS */;
INSERT INTO `State` VALUES (1,1,'Enqueued','Triggered by recurring job scheduler','2025-03-13 07:49:47.197229','{\"EnqueuedAt\":\"2025-03-13T07:49:47.1433018Z\",\"Queue\":\"default\"}'),(2,1,'Processing',NULL,'2025-03-13 07:50:01.443982','{\"StartedAt\":\"2025-03-13T07:50:01.4077173Z\",\"ServerId\":\"msi:7216:3f31e98c-c4a2-4071-b8d6-386e1a0f4245\",\"WorkerId\":\"b3018df7-3357-44a4-8ab9-b10d560c1f94\"}'),(3,1,'Succeeded',NULL,'2025-03-13 07:50:01.549971','{\"SucceededAt\":\"2025-03-13T07:50:01.5098519Z\",\"PerformanceDuration\":\"27\",\"Latency\":\"14446\"}'),(4,2,'Enqueued','Triggered by recurring job scheduler','2025-03-13 08:04:29.180299','{\"EnqueuedAt\":\"2025-03-13T08:04:29.1565087Z\",\"Queue\":\"default\"}'),(5,2,'Processing',NULL,'2025-03-13 08:04:43.939365','{\"StartedAt\":\"2025-03-13T08:04:43.8952758Z\",\"ServerId\":\"msi:5776:ae0b434b-38a9-4653-9b50-b9dc516a2d8d\",\"WorkerId\":\"a0c3e467-05d6-4219-9344-4bc293ea070e\"}'),(6,2,'Succeeded',NULL,'2025-03-13 08:04:44.052592','{\"SucceededAt\":\"2025-03-13T08:04:44.0118968Z\",\"PerformanceDuration\":\"27\",\"Latency\":\"14885\"}'),(7,3,'Enqueued','Triggered by recurring job scheduler','2025-03-13 10:06:01.904727','{\"EnqueuedAt\":\"2025-03-13T10:06:01.8864159Z\",\"Queue\":\"default\"}'),(8,3,'Processing',NULL,'2025-03-13 10:06:16.733759','{\"StartedAt\":\"2025-03-13T10:06:16.7029111Z\",\"ServerId\":\"aa34c7fcc898:857:0e904b69-e9fd-4e8d-a48d-e9b03f9635d1\",\"WorkerId\":\"3c8460a1-2101-44ca-98bd-4515c3c5ae64\"}'),(9,3,'Succeeded',NULL,'2025-03-13 10:06:16.809775','{\"SucceededAt\":\"2025-03-13T10:06:16.7840713Z\",\"PerformanceDuration\":\"15\",\"Latency\":\"14928\"}');
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
INSERT INTO `Therapists` VALUES ('647bc784-94f6-47e0-9ba1-1891f97c4f28',1,'This is Ms. Harley Ferdinand',0,'0185adf5-68a9-4e17-996f-15dae2589637','2025-03-13 05:02:35.968557','2025-03-13 05:02:35.968557'),('843f9d1b-e5b3-4bbf-83ab-0d5fd8d56b47',1,'This is a description field',0,'69caf7a7-1701-45d5-8d6f-063c518739fd','2025-03-13 05:02:35.870806','2025-03-13 05:02:35.870806'),('aeb59c18-9706-4cf6-9054-d19e05612c7c',2,'This is Mr. Tommy Vercetti',0,'1bbe3b5c-e4d8-4303-bc8b-2600102fd0f3','2025-03-13 05:02:35.968933','2025-03-13 05:02:35.968933'),('c5fb1261-fdda-431d-bda2-ce8dab497349',5,'This is therapist 2',0,'c515bc92-b256-4dc3-a3a4-427c2e38dba8','2025-03-13 05:02:35.967270','2025-03-13 05:02:35.967270');
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
  `transaction_time` datetime NOT NULL,
  `transaction_value` decimal(16,2) NOT NULL,
  `payment_method` int NOT NULL,
  `transaction_code` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
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
-- Table structure for table `UserChoice`
--

DROP TABLE IF EXISTS `UserChoice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserChoice` (
  `id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `QuestionOptionID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserID` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `last_update` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_UserChoice_QuestionOptionID` (`QuestionOptionID`),
  KEY `IX_UserChoice_UserID` (`UserID`),
  CONSTRAINT `FK_UserChoice_QuestionOptions_QuestionOptionID` FOREIGN KEY (`QuestionOptionID`) REFERENCES `QuestionOptions` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_UserChoice_Users_UserID` FOREIGN KEY (`UserID`) REFERENCES `Users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserChoice`
--

LOCK TABLES `UserChoice` WRITE;
/*!40000 ALTER TABLE `UserChoice` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserChoice` ENABLE KEYS */;
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
INSERT INTO `Users` VALUES ('0185adf5-68a9-4e17-996f-15dae2589637','Khánh Linh','Therapist_03','example_therapist_1@gmail.com','AA/1bC4VhhOILVWR4ifhymc8tDTYGb82ynhwJxrSYrgEKi6V2esVwFJu7bjgN+fLZjAYKOqSutE=','1980-11-30',1,'7429486726','Therapist','',0,'2025-03-13 05:02:35.968534','2025-03-13 05:02:35.968535'),('08dd61ec-4529-4907-8000-49cea6f07271','','Admin','vcl@gmail.com','sfUgaMRpt5vkNYZuz9zwlPLKei+GsqyfQ7ynv1TDeFZlTm3fVv9mDa10mmWnq5VyecYWfNRHQmI=','0001-01-01',0,'','Admin','',0,'2025-03-13 05:02:35.557936','2025-03-13 05:02:35.557937'),('08dd61ec-453f-458e-800a-85bc417218fb','Tran Nguyen Quoc Viet','Manager','sample@example.com','AA/1bC4VhhOILVWR4ifhymc8tDTYGb82ynhwJxrSYrgEKi6V2esVwFJu7bjgN+fLZjAYKOqSutE=','0001-01-01',1,'','Manager','',0,'2025-03-13 05:02:35.841686','2025-03-13 05:02:35.841687'),('1bbe3b5c-e4d8-4303-bc8b-2600102fd0f3','Đức Minh','Therapist_5','example_therapist_2@gmail.com','AA/1bC4VhhOILVWR4ifhymc8tDTYGb82ynhwJxrSYrgEKi6V2esVwFJu7bjgN+fLZjAYKOqSutE=','1991-11-30',0,'188928777','Therapist','',0,'2025-03-13 05:02:35.968918','2025-03-13 05:02:35.968918'),('22ec91ef-e556-4271-b9ce-ef739b440326','Hữu Phước','Customer_03','example03@gmail.com','AA/1bC4VhhOILVWR4ifhymc8tDTYGb82ynhwJxrSYrgEKi6V2esVwFJu7bjgN+fLZjAYKOqSutE=','2003-08-11',0,'0997442823','Customer','',0,'2025-03-13 05:02:35.969570','2025-03-13 05:02:35.969571'),('64033f03-9aa1-4a97-a4fb-7ad25020e4ff','Ngọc Sơn','Customer_02','example02@gmail.com','AA/1bC4VhhOILVWR4ifhymc8tDTYGb82ynhwJxrSYrgEKi6V2esVwFJu7bjgN+fLZjAYKOqSutE=','2004-12-02',0,'0997442823','Customer','',0,'2025-03-13 05:02:35.969340','2025-03-13 05:02:35.969340'),('69caf7a7-1701-45d5-8d6f-063c518739fd','Gia Huy','Therapist_1','example04@gmail.com','AA/1bC4VhhOILVWR4ifhymc8tDTYGb82ynhwJxrSYrgEKi6V2esVwFJu7bjgN+fLZjAYKOqSutE=','1997-04-20',0,'9912448336','Therapist','',0,'2025-03-13 05:02:35.870223','2025-03-13 05:02:35.870223'),('6fb32ebd-75d0-48b3-96e2-bce210e05126','Bảo Ngọc','Staff_02','example07@gmail.com','AA/1bC4VhhOILVWR4ifhymc8tDTYGb82ynhwJxrSYrgEKi6V2esVwFJu7bjgN+fLZjAYKOqSutE=','2000-05-25',1,'1693837522','Staff','',0,'2025-03-13 05:02:35.869963','2025-03-13 05:02:35.869963'),('82a37717-6447-41c5-8a21-bb86a38ccf27','Tuấn Kiệt','Customer_01','example01@gmail.com','AA/1bC4VhhOILVWR4ifhymc8tDTYGb82ynhwJxrSYrgEKi6V2esVwFJu7bjgN+fLZjAYKOqSutE=','2004-12-02',1,'0194302421','Customer','',0,'2025-03-13 05:02:35.969213','2025-03-13 05:02:35.969214'),('8b24e276-7df0-4fa2-aabe-e911f856795a','Anh Thư','Staff_01','staff01@gmail.com','AA/1bC4VhhOILVWR4ifhymc8tDTYGb82ynhwJxrSYrgEKi6V2esVwFJu7bjgN+fLZjAYKOqSutE=','1980-12-31',1,'324563447','Staff','',0,'2025-03-13 05:02:35.844201','2025-03-13 05:02:35.844202'),('c515bc92-b256-4dc3-a3a4-427c2e38dba8','Nguyen Van Lai','Therapist_02','example05@gmail.com','AA/1bC4VhhOILVWR4ifhymc8tDTYGb82ynhwJxrSYrgEKi6V2esVwFJu7bjgN+fLZjAYKOqSutE=','1997-04-20',0,'8912448336','Therapist','',0,'2025-03-13 05:02:35.967226','2025-03-13 05:02:35.967227');
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
INSERT INTO `__EFMigrationsHistory` VALUES ('20250313050101_minhtri','8.0.13');
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

-- Dump completed on 2025-03-13 17:17:20
