@echo off&setlocal enabledelayedexpansion

rem author: ������
rem date  : 2016-04-13

echo *-=-=-=-=-=-=-=-=-=-=-=  =-=-=-=-=-=-=-=-=-=-=-*
echo * ʹ�÷�����                                   *
echo * 1�������ļ�����Ҫ������ļ�����              *
echo * 2��ִ�и��ļ�                                *
echo *                                              *
echo * �����ɰ�����ʾ�ϴ��� NuGet ������          *
echo *-=-=-=-=-=-=-=-=-=-=-=  =-=-=-=-=-=-=-=-=-=-=-*
pause

rem ���õ�ǰ��ĿĿ¼
set libraryDir=%~dp0
%~d0
cd %libraryDir%
goto findNuGetDir

:cdParent
rem ����һ��Ŀ¼
cd..

:findNuGetDir
rem �ҵ� NuGet Ŀ¼
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
rem ���� NuGet.exe �ļ�ȫ·��
echo ���ڲ��� NuGet.exe �ļ�...
set count=0
for /f "delims=" %%a in ('dir /b /s /a-d-h-s NuGet.exe') do (
set /a count+=1
set nugetpath=%%a
)

if !count!==0 (
echo ��·����%cd����δ�ҵ� NuGet.exe �ļ�
goto end
)

:createPackage
echo 
echo -=-=-=-=-=-= �����ĿΪ nupkg �ļ� =-=-=-=-=-=-
rem �����ĿΪ nupkg �ļ�
echo ���ڲ��� *.nuspec �ļ�...
cd %libraryDir%
for /f "delims=" %%a in ('dir /b /a-d *.nuspec') do (
echo ���ڴ����%%a...
%nugetpath% pack %%a
)

echo  
echo -=-=-=-=-=-= ��ֵ����ļ��� NuGet �������� =-=-=-=-=-=-
rem ��ֵ����ļ��� NuGet ��������
set /p copyToServer=�Ƿ��Ƶ��������ϣ�y/n����
if "!copyToServer!"=="y" (
echo ���ڿ����ļ���*.nupkg���� ��\\192.168.3.28\NuGet_Local_Server\Packages\��...
copy /-Y *.nupkg \\192.168.3.28\NuGet_Local_Server\Packages\
echo ������ɡ�
)

:end
rem ����
pause