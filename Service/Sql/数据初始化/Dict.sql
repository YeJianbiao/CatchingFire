If Not Exists(Select '1' From dict Where code = 'client')
Begin
	select * From dict
	insert into dict (Id,code,value,value_1,value_2,value_3) values(NEWID(),'client','YeJianbiao','kfwgisfi','','')
End