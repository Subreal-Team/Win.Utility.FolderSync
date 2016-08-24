@rem ���������� paket
@echo .\.paket\paket.bootstrapper.exe
@.\.paket\paket.bootstrapper.exe
@if ERRORLEVEL 1 goto err

@setlocal
@set AGAIN=1
:start

	@rem �������� ������� (������� �������� ����� �� ������� �������)
    @echo ^> remove SubrealTeam packages
	@for /d %%i in (".\packages\SubrealTeam*") do @del /f /S /q "%%i" >nul & @rmdir /s /q "%%i" >nul 2>&1
	@for /d %%i in (".\packages\SubrealTeam*") do @rmdir /s /q "%%i" >nul

	@echo ^>
	@echo ^>

	@rem ���������� ������������
	@echo ^> .\.paket\paket update
	@.\.paket\paket update
	@if ERRORLEVEL 1 goto tryagain

	@rem ���� ��������� �������������� ���

@goto exit


:tryagain
	@rem ��������� �� ��� ���
	@if %AGAIN%==0 goto err

	@set /a AGAIN-=1
	@rem ����� ERRORLEVEL � 0
	@cd .

	@echo ^>
	@echo ^>
	@echo ^> Close Visual Studio and press Enter, I'll try again
	@pause
@goto start


:err
	@echo ^>
	@echo ^>
	@echo ^> I'm so sorry... I failed...
@goto exit

:exit
	@pause
	@endlocal