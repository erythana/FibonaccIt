Imports System
Imports System.ComponentModel.Design
Imports System.Globalization
Imports Microsoft.VisualBasic
Imports Microsoft.VisualStudio.Shell
Imports Microsoft.VisualStudio.Shell.Interop

''' <summary>
''' Command handler
''' </summary>
Public NotInheritable Class RunFormatter

    ''' <summary>
    ''' Command ID.
    ''' </summary>
    Public Const CommandId As Integer = 256

    ''' <summary>
    ''' Command menu group (command set GUID).
    ''' </summary>
    Public Shared ReadOnly CommandSet As New Guid("fd35cb35-f55b-4ac4-872f-f7ae6941b432")

    ''' <summary>
    ''' VS Package that provides this command, not null.
    ''' </summary>
    Private ReadOnly package As package

    ''' <summary>
    ''' Initializes a new instance of the <see cref="RunFormatter"/> class.
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
    Public Shared Property Instance As RunFormatter

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
        Instance = New RunFormatter(package)
    End Sub

    ''' <summary>
    ''' This function is the callback used to execute the command when the menu item is clicked.
    ''' See the constructor to see how the menu item is associated with this function using
    ''' OleMenuCommandService service and MenuCommand class.
    ''' </summary>
    ''' <param name="sender">Event sender.</param>
    ''' <param name="e">Event args.</param>
    ''' 

    'Declaration

    Private Sub MenuItemCallback(sender As Object, e As EventArgs)

        ThreadHelper.ThrowIfNotOnUIThread()

        'Fibonacci-Magic goes here
        Dim dte As EnvDTE.DTE = TryCast(Package.GetGlobalService(GetType(EnvDTE.DTE)), EnvDTE.DTE)

        If dte.ActiveDocument IsNot Nothing Then

            'turning smart indentation OFF
            TurnOffFormatting(dte)


            Dim activeDoc As EnvDTE.TextDocument = TryCast(dte.ActiveDocument.Object(), EnvDTE.TextDocument)


            activeDoc.Selection.SelectAll()
            activeDoc.Selection.SmartFormat()
            Dim indentsize As Integer = activeDoc.IndentSize
            Dim indentdepth As Integer = 0

            Dim replaceText As String = ""

            'backup code from window before changing anything
            Dim selText As String = activeDoc.Selection.Text

            For Each line In activeDoc.Selection.Text.Split(Environment.NewLine)
                'Here i come, /r/ProgrammerHumor - garbage code to remove the vbLf (probably) from the split above
                line = line.Replace(Chr(10), "")

                indentdepth = (line.Length - line.TrimStart.Length) / indentsize

                Dim spacenumber = 0
                For i As Integer = indentdepth To 0 Step -1
                    spacenumber += FibonacciIt(i)
                Next
                replaceText = replaceText & Space(spacenumber) & line.TrimStart & vbNewLine
            Next
            activeDoc.Selection.Delete()
            activeDoc.Selection.Insert(replaceText)

        End If


    End Sub

    Private Function FibonacciIt(ByVal number As Integer) As Integer
        If number > 2 Then
            number = FibonacciIt(number - 1) + (FibonacciIt(number - 2))
        Else
            number = 1
        End If

        Return number
    End Function

    Private Sub TurnOffFormatting(ByVal dte As EnvDTE.DTE)

        Dim p1 As EnvDTE.Properties = dte.Properties("TextEditor", "Basic-Specific")
        p1.Item("PrettyListing").Value = False

        Dim p2 As EnvDTE.Properties = dte.Properties("TextEditor", "AllLanguages")
        p2.Item("InsertTabs").Value = False
    End Sub

End Class



