INSERT INTO Inventory (inventoryId, VendorId, BaseCurrency, QuoteCurrency, Rate, Available, DateCreated)
VALUES
(newid(),'VENDOR_A', 'USD', 'EUR', 1.25, 5000,getdate()),
(newid(),'VENDOR_B', 'USD', 'EUR', 1.22, 3000,getdate()),
(newid(),'VENDOR_C', 'USD', 'EUR', 1.28, 10000,getdate()),
(newid(),'VENDOR_D', 'USD', 'GBP', 0.86, 8000,getdate()),
(newid(),'VENDOR_E', 'USD', 'GBP', 0.84, 6000,getdate());



SELECT * FROM Inventory with(nolock) order by vendorid asc


truncate table Inventory

