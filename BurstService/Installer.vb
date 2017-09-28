Imports System.ServiceProcess
Imports System.ComponentModel
Imports System.Configuration.Install


<RunInstaller(True)> Public Class prjInstaller
    Inherits Installer

    Public Sub New()
        MyBase.New()
        InitializeComponent()
    End Sub

    Friend WithEvents process As ServiceProcessInstaller
    Friend WithEvents serviceAdmin As ServiceInstaller
    Private components As Container

    <DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.process = New ServiceProcessInstaller()
        Me.process.Account = ServiceAccount.LocalSystem

        Me.serviceAdmin = New ServiceInstaller()
        Me.serviceAdmin.StartType = ServiceStartMode.Automatic
        Me.serviceAdmin.ServiceName = "Burst Service"
        Me.serviceAdmin.DisplayName = "Burst Service"
        Me.Installers.AddRange(New Installer() {Me.process, Me.serviceAdmin})
    End Sub



End Class