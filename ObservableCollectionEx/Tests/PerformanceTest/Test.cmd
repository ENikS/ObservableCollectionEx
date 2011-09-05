@REM initialize test value to be "true"
@SET intConsumers=1

:again

@SET intCounter=1

:while

@REM test condition
@IF %intCounter% GTR 30 (GOTO wend)
@IF %intConsumers% GTR 10 (GOTO done)

PerformanceTest.exe %intConsumers% >> test%intConsumers%.txt

@REM set new test value
@SET /a intCounter=intCounter+1


@REM loop
@GOTO while

:wend

@SET /a intConsumers=intConsumers+intConsumers


@REM loop
@GOTO again


:done