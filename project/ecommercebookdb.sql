-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 12, 2025 at 01:38 PM
-- Server version: 10.4.25-MariaDB
-- PHP Version: 7.4.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ecommercebookdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `bannerannouncements`
--

CREATE TABLE `bannerannouncements` (
  `Id` char(36) NOT NULL,
  `Title` longtext NOT NULL,
  `Message` longtext NOT NULL,
  `StartTime` datetime(6) NOT NULL,
  `EndTime` datetime(6) NOT NULL,
  `IsActive` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `bookmarks`
--

CREATE TABLE `bookmarks` (
  `Id` char(36) NOT NULL,
  `UserId` char(36) NOT NULL,
  `BookId` char(36) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `bookmarks`
--

INSERT INTO `bookmarks` (`Id`, `UserId`, `BookId`, `CreatedAt`) VALUES
('4471fbbb-2ca7-4e1f-a78f-0bc50a7bbf4b', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255', '805c4ef5-f39a-4f4b-80b8-4d07858ff31b', '2025-05-11 15:40:19.398990');

-- --------------------------------------------------------

--
-- Table structure for table `books`
--

CREATE TABLE `books` (
  `Id` char(36) NOT NULL,
  `Title` longtext NOT NULL,
  `Author` longtext NOT NULL,
  `ISBN` longtext NOT NULL,
  `Price` decimal(18,2) NOT NULL,
  `StockQuantity` int(11) NOT NULL,
  `IsOnSale` tinyint(1) NOT NULL,
  `SaleStartDate` datetime(6) DEFAULT NULL,
  `SaleEndDate` datetime(6) DEFAULT NULL,
  `SalePrice` decimal(18,2) DEFAULT NULL,
  `Category` longtext NOT NULL,
  `BookImageUrl` longtext NOT NULL,
  `Tags` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `books`
--

INSERT INTO `books` (`Id`, `Title`, `Author`, `ISBN`, `Price`, `StockQuantity`, `IsOnSale`, `SaleStartDate`, `SaleEndDate`, `SalePrice`, `Category`, `BookImageUrl`, `Tags`) VALUES
('091e2cf8-fb36-4c3d-be01-e8f0b290ab9e', 'Book1', 'Author1', 'ISHB', '123.00', 448, 0, NULL, NULL, NULL, 'lksdfjsldf', '', '[\"sdfsdf\"]'),
('805c4ef5-f39a-4f4b-80b8-4d07858ff31b', 'TIteee', 'autho', 'isbn', '123.00', 12297, 0, NULL, NULL, NULL, 'kljsdflkds', '/images/books/8b8683f1-4d8f-4b20-b49f-20357871ab75.png', '[\"sdfsdf\"]'),
('b3a56fbf-c0d5-48c0-9d0d-a2450b867d8b', 'sdflsdflj', 'lkjslkdfjl', 'kljsdklfj', '3234.00', 2334207, 0, NULL, NULL, NULL, 'sdfljsdlf', '/images/books/34d64479-4b1b-4ac8-87c5-7e773ab9e75e.png', '[\"sdfsdf\"]');

-- --------------------------------------------------------

--
-- Table structure for table `cartitems`
--

CREATE TABLE `cartitems` (
  `Id` char(36) NOT NULL,
  `CartId` char(36) NOT NULL,
  `BookId` char(36) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `Price` decimal(18,2) NOT NULL,
  `BookId1` char(36) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `carts`
--

CREATE TABLE `carts` (
  `Id` char(36) NOT NULL,
  `UserId` char(36) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `IsActive` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `carts`
--

INSERT INTO `carts` (`Id`, `UserId`, `CreatedDate`, `IsActive`) VALUES
('19606f2c-d9f5-4fbe-8a97-42347fd12db7', 'eef09f7f-c5dc-463c-a749-7612d1ff3854', '2025-05-10 15:59:46.035388', 0),
('2658c507-bd19-461b-971d-8eb193667771', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255', '2025-05-11 14:45:23.180830', 0);

-- --------------------------------------------------------

--
-- Table structure for table `orderitems`
--

CREATE TABLE `orderitems` (
  `Id` char(36) NOT NULL,
  `OrderId` char(36) NOT NULL,
  `BookId` char(36) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `BookTitle` longtext NOT NULL,
  `Price` decimal(18,2) NOT NULL DEFAULT 0.00
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `orderitems`
--

INSERT INTO `orderitems` (`Id`, `OrderId`, `BookId`, `Quantity`, `BookTitle`, `Price`) VALUES
('025fed84-26c7-447c-b841-47a4b84f95f3', '2217709d-a86b-42d0-ba8b-e188ce117d13', '805c4ef5-f39a-4f4b-80b8-4d07858ff31b', 1, '', '123.00'),
('18809efd-4ccc-42fe-81ed-940644b939b3', '4d3d13ef-7eee-4f4d-a211-5b29c739dcea', '805c4ef5-f39a-4f4b-80b8-4d07858ff31b', 1, '', '123.00'),
('1f63bbd2-9c83-4f04-b0a3-74a4f378df74', '5fa7b601-dd16-4908-9713-52c7c3afdc0d', '805c4ef5-f39a-4f4b-80b8-4d07858ff31b', 1, '', '123.00'),
('26c1c5f5-9f08-422c-a651-434363e2e713', 'b2498303-d635-49ab-8c69-cfc55dd367bb', '805c4ef5-f39a-4f4b-80b8-4d07858ff31b', 1, '', '123.00'),
('2ae043cf-cfc4-41cb-a3f3-4dee6ed9efcc', '0b2f37d2-1ca4-4c0b-83da-f282d3f3629b', 'b3a56fbf-c0d5-48c0-9d0d-a2450b867d8b', 17, '', '3234.00'),
('420daa73-a4b4-4a89-871c-c8d6d93e7a09', '10987231-a097-40ad-92c4-3117eeacc07a', '805c4ef5-f39a-4f4b-80b8-4d07858ff31b', 1, '', '123.00'),
('520eaeb7-6456-49c0-a591-b9bb9b49598e', '0b2f37d2-1ca4-4c0b-83da-f282d3f3629b', '805c4ef5-f39a-4f4b-80b8-4d07858ff31b', 9, '', '123.00'),
('65fa5072-099f-4ac6-81c8-7d0d67d4439b', '995c0c83-c8c4-4b25-b02a-4356bad3d45d', 'b3a56fbf-c0d5-48c0-9d0d-a2450b867d8b', 9, '', '3234.00'),
('7ace9ec9-89f1-4b73-b3b2-ce50db83d070', 'b36f9ac3-4c45-4107-ab25-7cb87580cb20', '091e2cf8-fb36-4c3d-be01-e8f0b290ab9e', 1, '', '123.00'),
('a3a6c2f6-b787-4c54-a53b-21d55c7b5fc8', '995c0c83-c8c4-4b25-b02a-4356bad3d45d', '805c4ef5-f39a-4f4b-80b8-4d07858ff31b', 1, '', '123.00'),
('af72ed4a-c414-4bd7-8bf8-05098322e1da', '10987231-a097-40ad-92c4-3117eeacc07a', 'b3a56fbf-c0d5-48c0-9d0d-a2450b867d8b', 1, '', '3234.00'),
('c1401572-5ad2-458e-9f95-be3cbe885e33', '7680df13-0a27-4ae2-bb94-4cb1e1b30a8b', '091e2cf8-fb36-4c3d-be01-e8f0b290ab9e', 1, '', '123.00'),
('ea42a93f-5456-4d2f-8783-4dcadac77a32', 'b9019a17-a700-4f4e-bea5-7e83c012c5df', '091e2cf8-fb36-4c3d-be01-e8f0b290ab9e', 5, '', '123.00');

-- --------------------------------------------------------

--
-- Table structure for table `orders`
--

CREATE TABLE `orders` (
  `Id` char(36) NOT NULL,
  `UserId` char(36) NOT NULL,
  `OrderDate` datetime(6) NOT NULL,
  `Status` int(11) NOT NULL,
  `TotalPrice` decimal(18,2) NOT NULL,
  `DiscountTotal` decimal(18,2) NOT NULL,
  `ClaimCode` longtext NOT NULL,
  `IsFulfilled` tinyint(1) NOT NULL,
  `CancelledDate` datetime(6) DEFAULT NULL,
  `FulfilledDate` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `orders`
--

INSERT INTO `orders` (`Id`, `UserId`, `OrderDate`, `Status`, `TotalPrice`, `DiscountTotal`, `ClaimCode`, `IsFulfilled`, `CancelledDate`, `FulfilledDate`) VALUES
('0b2f37d2-1ca4-4c0b-83da-f282d3f3629b', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255', '2025-05-11 16:43:52.266997', 2, '53280.75', '2804.25', '46BD8797298A', 0, NULL, NULL),
('10987231-a097-40ad-92c4-3117eeacc07a', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255', '2025-05-11 17:18:23.545262', 2, '3357.00', '0.00', 'D97DAA02BE17', 0, NULL, NULL),
('2217709d-a86b-42d0-ba8b-e188ce117d13', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255', '2025-05-11 16:42:07.629001', 2, '123.00', '0.00', '03FD0295F723', 0, NULL, NULL),
('4d3d13ef-7eee-4f4d-a211-5b29c739dcea', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255', '2025-05-11 17:17:43.909113', 2, '123.00', '0.00', '4BE9CD177602', 0, NULL, NULL),
('5fa7b601-dd16-4908-9713-52c7c3afdc0d', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255', '2025-05-11 17:19:02.748988', 2, '123.00', '0.00', '451501021188', 0, NULL, NULL),
('7680df13-0a27-4ae2-bb94-4cb1e1b30a8b', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255', '2025-05-11 17:18:02.517619', 2, '123.00', '0.00', '38500BED5944', 0, NULL, NULL),
('995c0c83-c8c4-4b25-b02a-4356bad3d45d', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255', '2025-05-11 16:47:19.871519', 2, '27767.55', '1461.45', '75ED579CA937', 0, NULL, NULL),
('b2498303-d635-49ab-8c69-cfc55dd367bb', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255', '2025-05-11 17:18:43.810548', 2, '123.00', '0.00', '346114CB0549', 0, NULL, NULL),
('b36f9ac3-4c45-4107-ab25-7cb87580cb20', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255', '2025-05-10 16:52:55.212566', 2, '123.00', '0.00', '8CC5119B0D8C', 0, NULL, NULL),
('b9019a17-a700-4f4e-bea5-7e83c012c5df', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255', '2025-05-11 14:45:39.245751', 2, '584.25', '30.75', '3C23A1092FA4', 0, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `reviews`
--

CREATE TABLE `reviews` (
  `Id` char(36) NOT NULL,
  `Rating` int(11) NOT NULL,
  `Comment` longtext NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `BookId` char(36) NOT NULL,
  `UserId` char(36) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `reviews`
--

INSERT INTO `reviews` (`Id`, `Rating`, `Comment`, `CreatedAt`, `UpdatedAt`, `BookId`, `UserId`) VALUES
('370e1d97-a356-44f9-96bb-a1d7ef629f23', 5, 'kdjfklsdf', '2025-05-11 15:52:27.772182', NULL, '091e2cf8-fb36-4c3d-be01-e8f0b290ab9e', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255'),
('920c4cbb-94f1-4175-bdbd-0b12c8f88ecc', 2, 'string', '2025-05-11 15:28:49.547546', NULL, '091e2cf8-fb36-4c3d-be01-e8f0b290ab9e', 'ce9550e4-c0b1-46f8-ae02-c7634dcc4255');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `Id` char(36) NOT NULL,
  `Email` longtext NOT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `Role` int(11) NOT NULL,
  `SuccessfulOrders` int(11) NOT NULL,
  `FullName` longtext NOT NULL,
  `ProfileImageUrl` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`Id`, `Email`, `PasswordHash`, `Role`, `SuccessfulOrders`, `FullName`, `ProfileImageUrl`) VALUES
('74195319-bc0b-4f8c-9d5e-e32eb46aaae5', 'staff@admin.com', 'AQAAAAIAAYagAAAAEAqf1HE5GmZMd0F8ZyfVWB8NX+aDRlpUppXsOPuPNsivZ3nArSE+zd0YhWShKUPlXw==', 1, 0, 'sdfsdfsdf', '/images/profiles/aea03dbf-703d-409d-a89b-f6aece97d670.jpg'),
('ce9550e4-c0b1-46f8-ae02-c7634dcc4255', 'summitshan@gmail.com', 'AQAAAAIAAYagAAAAEDBDQcjVtgX1fwKaIC45le8rxtcw4fuvWXd/8U+G+WUcg4GN6hnIGrDuQMjHHurtUA==', 0, 0, 'Hii Gmail', '/images/profiles/0a322598-a19e-426e-9d0e-621b11e677c3.jpeg'),
('d434c5df-56bc-4e62-8138-f7b10cdf9d33', 'admin@admin.com', 'AQAAAAIAAYagAAAAEJj86PjnMpdzBqN+PdV85cu4ALCit1JvEYda0QSYyzM2aWK+LPSKPxXRnfpmQv9zYg==', 2, 0, 'Admin', '/images/profiles/4a6f8efe-99a4-492e-9e7d-c603f88cdebd.png'),
('eef09f7f-c5dc-463c-a749-7612d1ff3854', 'fullname@gmail.com', 'AQAAAAIAAYagAAAAELSfM1l9ImB+2i0NYn2D/LT+EEl/aaMp94IoUrHPuueHdhfkElS4ewLIWlBVyuPkuw==', 2, 0, 'fullname', '/images/profiles/ad55e311-1a42-4e7a-8d22-e4b55d3b200a.jpg');

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20250420161237_InitialCreate', '8.0.11'),
('20250420181629_iInitialCreate', '8.0.11'),
('20250510105756_AdssssssssdUpdatedModels', '8.0.11'),
('20250510150855_AdssssssssdUpdatedModelsdfsdfsdfs', '8.0.11'),
('20250510155421_DSFSDAdssssssssdUpdatedModelsdfsdfsdfs', '8.0.11'),
('20250510165104_dfsdfAdssssssssdUpdatedModelsdfsdfsdfs', '8.0.11');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bannerannouncements`
--
ALTER TABLE `bannerannouncements`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `bookmarks`
--
ALTER TABLE `bookmarks`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Bookmarks_BookId` (`BookId`),
  ADD KEY `IX_Bookmarks_UserId` (`UserId`);

--
-- Indexes for table `books`
--
ALTER TABLE `books`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `cartitems`
--
ALTER TABLE `cartitems`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_CartItems_BookId` (`BookId`),
  ADD KEY `IX_CartItems_BookId1` (`BookId1`),
  ADD KEY `IX_CartItems_CartId` (`CartId`);

--
-- Indexes for table `carts`
--
ALTER TABLE `carts`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Carts_UserId` (`UserId`);

--
-- Indexes for table `orderitems`
--
ALTER TABLE `orderitems`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_OrderItems_OrderId` (`OrderId`),
  ADD KEY `IX_OrderItems_BookId` (`BookId`);

--
-- Indexes for table `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Orders_UserId` (`UserId`);

--
-- Indexes for table `reviews`
--
ALTER TABLE `reviews`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Reviews_BookId` (`BookId`),
  ADD KEY `IX_Reviews_UserId` (`UserId`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `bookmarks`
--
ALTER TABLE `bookmarks`
  ADD CONSTRAINT `FK_Bookmarks_Books_BookId` FOREIGN KEY (`BookId`) REFERENCES `books` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Bookmarks_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `cartitems`
--
ALTER TABLE `cartitems`
  ADD CONSTRAINT `FK_CartItems_Books_BookId` FOREIGN KEY (`BookId`) REFERENCES `books` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_CartItems_Books_BookId1` FOREIGN KEY (`BookId1`) REFERENCES `books` (`Id`),
  ADD CONSTRAINT `FK_CartItems_Carts_CartId` FOREIGN KEY (`CartId`) REFERENCES `carts` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `carts`
--
ALTER TABLE `carts`
  ADD CONSTRAINT `FK_Carts_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `orderitems`
--
ALTER TABLE `orderitems`
  ADD CONSTRAINT `FK_OrderItems_Books_BookId` FOREIGN KEY (`BookId`) REFERENCES `books` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_OrderItems_Orders_OrderId` FOREIGN KEY (`OrderId`) REFERENCES `orders` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `FK_Orders_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `reviews`
--
ALTER TABLE `reviews`
  ADD CONSTRAINT `FK_Reviews_Books_BookId` FOREIGN KEY (`BookId`) REFERENCES `books` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Reviews_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
