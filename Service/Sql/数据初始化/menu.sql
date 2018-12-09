If Not Exists(Select '1' From menu)
Begin
  insert into menu (id,code,name,name_en,icon,parent_code,sequence,url,tag,is_common_use,status,created_by,created_time,updated_by,updated_time)
  values(NEWID(),'System_Manager',N'系统设置','System Setting','','',1,'','WPF_PAGE',0,'1','',getdate(),'',getdate()) 

  insert into menu (id,code,name,name_en,icon,parent_code,sequence,url,tag,is_common_use,status,created_by,created_time,updated_by,updated_time)
  values(NEWID(),'System_Navigation',N'导航菜单','Navigation','','System_Manager',10,'pack://application:,,,/CatchingFire;component/Area/Sys/View/Menu.xaml','WPF_PAGE',0,'1','',getdate(),'',getdate()) 

  insert into menu (id,code,name,name_en,icon,parent_code,sequence,url,tag,is_common_use,status,created_by,created_time,updated_by,updated_time)
  values(NEWID(),'System_User',N'账号管理','System User','','System_Manager',20,'pack://application:,,,/CatchingFire;component/Area/Sys/View/User.xaml','WPF_PAGE',0,'1','',getdate(),'',getdate()) 

  insert into menu (id,code,name,name_en,icon,parent_code,sequence,url,tag,is_common_use,status,created_by,created_time,updated_by,updated_time)
  values(NEWID(),'System_Log',N'日志记录','Log','','System_Manager',30,'pack://application:,,,/CatchingFire;component/Area/Sys/View/Log.xaml','WPF_PAGE',0,'1','',getdate(),'',getdate()) 

  insert into menu (id,code,name,name_en,icon,parent_code,sequence,url,tag,is_common_use,status,created_by,created_time,updated_by,updated_time)
  values(NEWID(),'Other',N'其它','Other','','',10,'','WPF_PAGE',1,'1','',getdate(),'',getdate()) 

  insert into menu (id,code,name,name_en,icon,parent_code,sequence,url,tag,is_common_use,status,created_by,created_time,updated_by,updated_time)
  values(NEWID(),'Other_Control',N'控件','Control','','Other',10,'pack://application:,,,/CatchingFire;component/Area/TechnologySummarize/View/Controls.xaml','WPF_PAGE',1,'1','',getdate(),'',getdate()) 

  insert into menu (id,code,name,name_en,icon,parent_code,sequence,url,tag,is_common_use,status,created_by,created_time,updated_by,updated_time)
  values(NEWID(),'Other_Icon',N'图标','Icon','','Other',30,'pack://application:,,,/CatchingFire;component/Area/FontTest/View/FontTest.xaml','WPF_PAGE',1,'1','',getdate(),'',getdate()) 

 

end
go

