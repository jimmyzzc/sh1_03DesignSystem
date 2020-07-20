@echo off&setlocal enabledelayedexpansion

rem author: 高启航
rem date  : 2016-04-13

echo *-=-=-=-=-=-=-=-=-=-=-=  =-=-=-=-=-=-=-=-=-=-=-*
echo * 使用方法：                                   *
echo * 1、将该文件放在要打包的文件夹中              *
echo * 2、执行该文件                                *
echo *                                              *
echo * 在生成包后将提示上传到 NuGet 服务器          *
echo *-=-=-=-=-=-=-=-=-=-=-=  =-=-=-=-=-=-=-=-=-=-=-*
pause

rem 设置当前项目目录
set libraryDir=%~dp0
%~d0
cd %libraryDir%
goto findNuGetDir

:cdParent
rem 向上一级目录
cd..

:findNuGetDir
rem 找到 NuGet 目录
set count=0
for /f "delims=" %%a in ('dir /b /ad 90PublicBin') do (
set /a count+=1
set !count!=%%a
)
if !count!==0 (
goto cdParent
) else (
goto setter
)

:setter
cls
rem 设置 NuGet.exe 文件全路径
echo 正在查找 NuGet.exe 文件...
set count=0
for /f "delims=" %%a in ('dir /b /s /a-d-h-s NuGet.exe') do (
set /a count+=1
set nugetpath=%%a
)

if !count!==0 (
echo 在路径“%cd”中未找到 NuGet.exe 文件
goto end
)

:createPackage
echo 
echo -=-=-=-=-=-= 打包项目为 nupkg 文件 =-=-=-=-=-=-
rem 打包项目为 nupkg 文件
echo 正在查找 *.nuspec 文件...
cd %libraryDir%
for /f "delims=" %%a in ('dir /b /a-d *.nuspec') do (
echo 正在打包：%%a...
%nugetpath% pack %%a
)

echo  
echo -=-=-=-=-=-= 赋值打包文件到 NuGet 服务器上 =-=-=-=-=-=-
rem 赋值打包文件到 NuGet 服务器上
set /p copyToServer=是否复制到服务器上（y/n）？
if "!copyToServer!"=="y" (
echo 正在拷贝文件“*.nupkg”到 “\\192.168.3.28\NuGet_Local_Server\Packages\”...
copy /-Y *.nupkg \\192.168.3.28\NuGet_Local_Server\Packages\
echo 拷贝完成。
)

:end
rem 结束
pause