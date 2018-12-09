If Not Exists(Select '1' From [YeSystem].[dbo].[user])
Begin
  insert into [YeSystem].[dbo].[user](id,name,pwd,user_code,status,created_by,created_time,updated_by,updated_time)
  values(NEWID(),'admin','admin','','1','',getdate(),'',getdate()) 

 insert into [YeSystem].[dbo].[user](id,name,pwd,user_code,status,created_by,created_time,updated_by,updated_time)
  values(NEWID(),'ye_jianbiao','kkfwgisfi','','1','',getdate(),'',getdate()) 

end
go

