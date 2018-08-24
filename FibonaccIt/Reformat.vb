Imports System
Imports System.ComponentModel.Design
Imports System.Globalization
Imports Microsoft.VisualStudio.Shell
Imports Microsoft.VisualStudio.Shell.Interop

''' <summary>
''' Command handler
''' </summary>
Public NotInheritable Class Reformat

    ''' <summary>
    ''' Command ID.
    ''' </summary>
    Public Const CommandId As Integer = 4129

    ''' <summary>
    ''' Command menu group (command set GUID).
    ''' </summary>
    Public Shared ReadOnly CommandSet As New Guid("ae28add6-7664-416d-937c-3935b4e146cb")

    ''' <summary>
    ''' VS Package that provides this command, not null.
    ''' </summary>
    Private ReadOnly package As package

    ''' <summary>
    ''' Initializes a new instance of the <see cref="Reformat"/> class.
    ''' Adds our command handlers for menu (the commands must exist in the command table file)
    ''' </summary>
    ''' <param name="package">Owner package, not null.</param>
    Private Sub New(package As package)
        If package Is Nothing Then
            Throw New ArgumentNullException("package")
        End If

        Me.package = package
        Dim commandService As OleMenuCommandService = Me.ServiceProvider.GetService(GetType(IMenuCommandService))
        If commandService IsNot Nothing Then
            Dim menuCommandId = New CommandID(CommandSet, CommandId)
            Dim menuCommand = New MenuCommand(AddressOf Me.MenuItemCallback, menuCommandId)
            commandService.AddCommand(menuCommand)
        End If
    End Sub

    ''' <summary>
    ''' Gets the instance of the command.
    ''' </summary>
    Public Shared Property Instance As Reformat

    ''' <summary>
    ''' Get service provider from the owner package.
    ''' </summary>
    Private ReadOnly Property ServiceProvider As IServiceProvider
        Get
            Return Me.package
        End Get
    End Property

    ''' <summary>
    ''' Initializes the singleton instance of the command.
    ''' </summary>
    ''' <param name="package">Owner package, Not null.</param>
    Public Shared Sub Initialize(package As package)
        Instance = New Reformat(package)
    End Sub

    ''' <summary>
    ''' This function is the callback used to execute the command when the menu item is clicked.
    ''' See the constructor to see how the menu item is associated with this function using
    ''' OleMenuCommandService service and MenuCommand class.
    ''' </summary>
    ''' <param name="sender">Event sender.</param>
    ''' <param name="e">Event args.</param>
    Private Sub MenuItemCallback(sender As Object, e As EventArgs)


        Dim dte As EnvDTE.DTE = TryCast(Package.GetGlobalService(GetType(EnvDTE.DTE)), EnvDTE.DTE)
        If dte.ActiveDocument IsNot Nothing Then
            'turning smart indentation OFF
            TurnOnFormatting(dte)

            Dim activeDoc As EnvDTE.TextDocument = TryCast(dte.ActiveDocument.Object(), EnvDTE.TextDocument)

            activeDoc.Selection.SelectAll()
            activeDoc.Selection.SmartFormat()

        End If

    End Sub

    Private Sub TurnOnFormatting(ByVal dte As EnvDTE.DTE)
 
        Dim p1 As EnvDTE.Properties = dte.Properties("TextEditor", "Basic-Specific")
        p1.Item("PrettyListing").Value = True

        Dim p2 As EnvDTE.Properties = dte.Properties("TextEditor", "AllLanguages")
        p2.Item("InsertTabs").Value = True

    End Sub
End Class
