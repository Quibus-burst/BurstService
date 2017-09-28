Imports System.Configuration.Install
Imports System.Reflection
Imports System.ServiceProcess

NotInheritable Class Program
    Private Sub New()
    End Sub
    ''' <summary>
    ''' The main entry point for the application.
    ''' </summary>
    Private Shared Sub Main()
        Dim _IsInstalled As Boolean = False
        Dim serviceStarting As Boolean = False
        ' Thanks to SMESSER's implementation V2.0
        Dim SERVICE_NAME As String = "Burst Service"

        Dim services As ServiceController() = ServiceController.GetServices()

        For Each service As ServiceController In services
            If service.ServiceName.Equals(SERVICE_NAME) Then
                _IsInstalled = True
                If service.Status = ServiceControllerStatus.StartPending Then
                    ' If the status is StartPending then the service was started via the SCM             
                    serviceStarting = True
                End If
                Exit For
            End If
        Next

        If Not serviceStarting Then
            If _IsInstalled = True Then
                ' Thanks to PIEBALDconsult's Concern V2.0
                Dim dr As New MsgBoxResult()
                dr = MsgBox((Convert.ToString("Do you REALLY like to uninstall the ") & SERVICE_NAME) + "?", MsgBoxStyle.YesNo)
                If dr = MsgBoxResult.Yes Then
                    SelfInstaller.UninstallMe()
                    MsgBox(Convert.ToString("Successfully uninstalled the ") & SERVICE_NAME, MsgBoxStyle.OkOnly Or MsgBoxStyle.Information)
                End If
            Else
                Dim dr As New MsgBoxResult()
                dr = MsgBox((Convert.ToString("Do you REALLY like to install the ") & SERVICE_NAME) + "?", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation)
                If dr = MsgBoxResult.Yes Then
                    SelfInstaller.InstallMe()
                    MsgBox(Convert.ToString("Successfully installed the ") & SERVICE_NAME, MsgBoxStyle.OkOnly Or MsgBoxStyle.Information)
                End If
            End If
        Else
            ' Started from the SCM
            Dim servicestorun As System.ServiceProcess.ServiceBase()
            servicestorun = New System.ServiceProcess.ServiceBase() {New Service1()}
            ServiceBase.Run(servicestorun)
        End If
    End Sub
End Class

Public NotInheritable Class SelfInstaller
    Private Sub New()
    End Sub
    Private Shared ReadOnly _exePath As String = Assembly.GetExecutingAssembly().Location
    Public Shared Function InstallMe() As Boolean
        Try
            ManagedInstallerClass.InstallHelper(New String() {_exePath})
        Catch
            Return False
        End Try
        Return True
    End Function

    Public Shared Function UninstallMe() As Boolean
        Try
            ManagedInstallerClass.InstallHelper(New String() {"/u", _exePath})
        Catch
            Return False
        End Try
        Return True
    End Function
End Class