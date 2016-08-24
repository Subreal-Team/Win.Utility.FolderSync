@rem Обновление paket
@echo .\.paket\paket.bootstrapper.exe
@.\.paket\paket.bootstrapper.exe
@if ERRORLEVEL 1 goto err

@setlocal
@set AGAIN=1
:start

	@rem Удаление пакетов (двойное удаление папок их реально удаляет)
    @echo ^> remove SubrealTeam packages
	@for /d %%i in (".\packages\SubrealTeam*") do @del /f /S /q "%%i" >nul & @rmdir /s /q "%%i" >nul 2>&1
	@for /d %%i in (".\packages\SubrealTeam*") do @rmdir /s /q "%%i" >nul

	@echo ^>
	@echo ^>

	@rem Обновление зависимостей
	@echo ^> .\.paket\paket update
	@.\.paket\paket update
	@if ERRORLEVEL 1 goto tryagain

	@rem сюда вставлять дополнительный код

@goto exit


:tryagain
	@rem Повторяем всё ещё раз
	@if %AGAIN%==0 goto err

	@set /a AGAIN-=1
	@rem сброс ERRORLEVEL в 0
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