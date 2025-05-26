SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE';

-- 1. Insert into Users table (5 rows, omitting Id for auto-increment)
INSERT INTO Users (FullName, Email, Password, Username)
VALUES
    ('John Doe', 'john.doe@example.com', 'Password123', 'johndoe'),
    ('Jane Smith', 'jane.smith@example.com', 'Password456', 'janesmith'),
    ('Alice Johnson', 'alice.johnson@example.com', 'Password789', 'alicej'),
    ('Bob Williams', 'bob.williams@example.com', 'Password101', 'bobw'),
    ('Emma Brown', 'emma.brown@example.com', 'Password202', 'emmab');

-- 2. Capture generated User IDs
DECLARE @UserIds TABLE (Id INT, Username NVARCHAR(255));
INSERT INTO @UserIds (Id, Username)
SELECT Id, Username FROM Users WHERE Username IN ('johndoe', 'janesmith', 'alicej', 'bobw', 'emmab');

-- 3. Insert into Profiles table (5 rows, omitting Id, linking to Users via UserId)
INSERT INTO Profiles (FullName, Email, PhotoUrl, Subscription, TrainingProgress, userScore, Achievements, UserId)
VALUES
    ('John Doe', 'john.doe@example.com', 'https://example.com/photos/john.jpg', 'Basic', 50, 100, 'First Login;Completed Training 1', (SELECT Id FROM @UserIds WHERE Username = 'johndoe')),
    ('Jane Smith', 'jane.smith@example.com', 'https://example.com/photos/jane.jpg', 'Premium', 75, 200, 'Completed Training 2;High Score', (SELECT Id FROM @UserIds WHERE Username = 'janesmith')),
    ('Alice Johnson', 'alice.johnson@example.com', 'https://example.com/photos/alice.jpg', 'Advanced', 30, 150, 'Joined Community;Training 3', (SELECT Id FROM @UserIds WHERE Username = 'alicej')),
    ('Bob Williams', 'bob.williams@example.com', 'https://example.com/photos/bob.jpg', 'Basic', 20, 50, 'First Login', (SELECT Id FROM @UserIds WHERE Username = 'bobw')),
    ('Emma Brown', 'emma.brown@example.com', 'https://example.com/photos/emma.jpg', 'Premium', 90, 300, 'Top Performer;Completed Training 4', (SELECT Id FROM @UserIds WHERE Username = 'emmab'));

-- 4. Capture generated Profile IDs
DECLARE @ProfileIds TABLE (Id INT, Username NVARCHAR(255));
INSERT INTO @ProfileIds (Id, Username)
SELECT p.Id, u.Username 
FROM Profiles p
JOIN @UserIds u ON p.UserId = u.Id;

-- 5. Insert into CertificateItem table (5 rows, linked to Profiles via ProfileId)
INSERT INTO CertificateItem (ProfileId, Title, PreviewUrl, DownloadUrl)
VALUES
    ((SELECT Id FROM @ProfileIds WHERE Username = 'johndoe'), 'VR Training 101', 'https://example.com/cert/preview1.jpg', 'https://example.com/cert/download1.pdf'),
    ((SELECT Id FROM @ProfileIds WHERE Username = 'johndoe'), 'Safety Basics', 'https://example.com/cert/preview2.jpg', 'https://example.com/cert/download2.pdf'),
    ((SELECT Id FROM @ProfileIds WHERE Username = 'janesmith'), 'Advanced VR Training', 'https://example.com/cert/preview3.jpg', 'https://example.com/cert/download3.pdf'),
    ((SELECT Id FROM @ProfileIds WHERE Username = 'alicej'), 'Factory Safety', 'https://example.com/cert/preview4.jpg', 'https://example.com/cert/download4.pdf'),
    ((SELECT Id FROM @ProfileIds WHERE Username = 'emmab'), 'Master VR Certification', 'https://example.com/cert/preview5.jpg', 'https://example.com/cert/download5.pdf');

-- 6. Insert into TrainingHistoryItem table (5 rows, linked to Profiles via ProfileId)
INSERT INTO TrainingHistoryItem (ProfileId, Title, Status)
VALUES
    ((SELECT Id FROM @ProfileIds WHERE Username = 'johndoe'), 'VR Training Module 1', 'Completed'),
    ((SELECT Id FROM @ProfileIds WHERE Username = 'janesmith'), 'VR Training Module 2', 'In Progress'),
    ((SELECT Id FROM @ProfileIds WHERE Username = 'alicej'), 'Safety Training', 'Completed'),
    ((SELECT Id FROM @ProfileIds WHERE Username = 'bobw'), 'Basic VR Intro', 'Not Started'),
    ((SELECT Id FROM @ProfileIds WHERE Username = 'emmab'), 'Advanced Safety Module', 'Completed');

--7. Insert into Plans table (5 rows, omitting Id for auto-increment)
CREATE TABLE #TempPlanIds (Id INT, Name NVARCHAR(100));
INSERT INTO Plans (Name, Description, Icon)
OUTPUT INSERTED.Id, INSERTED.Name INTO #TempPlanIds
VALUES
    ('Starter Plan', 'Basic access to VR training', 'star'),
    ('Pro Plan', 'Enhanced VR training features', 'pro'),
    ('Elite Plan', 'Full access to all scenarios', 'elite'),
    ('Team Plan', 'Multi-user VR training', 'team'),
    ('Enterprise Plan', 'Custom VR solutions', 'enterprise');

-- 8. Insert into PlanOption table (5 rows, linked to Plans, omitting Id)
CREATE TABLE #TempPlanOptionIds (Id INT, Label NVARCHAR(100));
INSERT INTO PlanOption (PlanId, DurationMonths, Label, Price, Note, PriceUnit)
OUTPUT INSERTED.Id, INSERTED.Label INTO #TempPlanOptionIds
VALUES
    ((SELECT Id FROM #TempPlanIds WHERE Name = 'Starter Plan'), 1, '1 Month Starter', 10.00, 'Basic access', 'EGP'),
    ((SELECT Id FROM #TempPlanIds WHERE Name = 'Pro Plan'), 6, '6 Month Pro', 250.00, 'Enhanced features', 'EGP'),
    ((SELECT Id FROM #TempPlanIds WHERE Name = 'Elite Plan'), 12, '1 Year Elite', 500.00, 'Full access', 'EGP'),
    ((SELECT Id FROM #TempPlanIds WHERE Name = 'Team Plan'), 3, '3 Month Team', 150.00, 'Team collaboration', 'EGP'),
    ((SELECT Id FROM #TempPlanIds WHERE Name = 'Enterprise Plan'), 12, '1 Year Enterprise', 1000.00, 'Custom solutions', 'EGP');

-- 9. Insert into PlanFeatures table (5 rows, linked to PlanOption, omitting Id)
INSERT INTO PlanFeatures (PlanOptionId, Description)
VALUES
    ((SELECT Id FROM #TempPlanOptionIds WHERE Label = '1 Month Starter'), 'Basic VR Training Access'),
    ((SELECT Id FROM #TempPlanOptionIds WHERE Label = '6 Month Pro'), 'Pro VR Scenarios'),
    ((SELECT Id FROM #TempPlanOptionIds WHERE Label = '1 Year Elite'), 'All Scenarios Unlocked'),
    ((SELECT Id FROM #TempPlanOptionIds WHERE Label = '3 Month Team'), 'Team Collaboration Tools'),
    ((SELECT Id FROM #TempPlanOptionIds WHERE Label = '1 Year Enterprise'), 'Custom VR Content Creation');





-- 11. Verify inserted data
SELECT 'Users' AS TableName, COUNT(*) AS [RowCount] FROM Users
UNION
SELECT 'Profiles', COUNT(*) FROM Profiles
UNION
SELECT 'CertificateItem', COUNT(*) FROM CertificateItem
UNION
SELECT 'TrainingHistoryItem', COUNT(*) FROM TrainingHistoryItem
UNION
SELECT 'Plans', COUNT(*) FROM Plans
UNION
SELECT 'PlanOption', COUNT(*) FROM PlanOption
UNION
SELECT 'PlanFeatures', COUNT(*) FROM PlanFeatures;


-- 12. Generate LeaderboardEntryDto data (derived from Profiles)
SELECT 
    ROW_NUMBER() OVER (ORDER BY userScore DESC) AS Rank,
    FullName,
    PhotoUrl,
    userScore AS Score
FROM Profiles
ORDER BY userScore DESC;