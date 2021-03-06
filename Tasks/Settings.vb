﻿Public Class Settings
    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim path As Drawing2D.GraphicsPath = FX.RoundedRectPath(ClientRectangle, 20)
        Region = New Region(path)
        FX.SetClassLong(Handle, FX.GCL_STYLE, FX.GetClassLong(Handle, FX.GCL_STYLE) Or FX.CS_DROPSHADOW)
        '加载主题
        BackColor = My.Settings.ThemeColor
        ForeColor = My.Settings.TForeColor
        Label.ForeColor = My.Settings.TForeColor
        Save_Button.BackColor = My.Settings.ThemeColor
        Save_Button.ForeColor = My.Settings.TForeColor
        Cancel_Button.BackColor = My.Settings.ThemeColor
        Cancel_Button.ForeColor = My.Settings.TForeColor
        Button1.BackColor = My.Settings.ThemeColor
        Button1.ForeColor = My.Settings.TForeColor
        Button_About.BackColor = My.Settings.ThemeColor
        Button_About.ForeColor = My.Settings.TForeColor
        Button_Help.BackColor = My.Settings.ThemeColor
        Button_Help.ForeColor = My.Settings.TForeColor
        PictureBox2.BackColor = My.Settings.ThemeColor
        PictureBox3.BackColor = My.Settings.TForeColor
        '加载设定
        TrackBar1.Value = My.Settings.NoticeLevel
        TrackBar2.Value = My.Settings.WarningLevel
        ComboBox1.SelectedItem = My.Settings.RemindInterval
        CheckBox1.Checked = My.Settings.RunWhenSysStart
        CheckBox2.Checked = My.Settings.HideWhenAutoRun
        CheckBox3.Checked = My.Settings.DecreaseAnimation

        DrawProgressBar()
    End Sub

    Private Sub DrawProgressBar()
        Dim bmp As Bitmap
        Dim gra As Graphics
        Dim ThemeColor As Color = My.Settings.ThemeColor
        Dim b As SolidBrush = New SolidBrush(ThemeColor) '普通的颜色
        Dim b1 As SolidBrush = New SolidBrush(Color.Orange) '注意的颜色
        Dim b2 As SolidBrush = New SolidBrush(Color.FromArgb(255, 0, 0)) '警告的颜色
        Dim NoticeLevel As Integer = My.Settings.NoticeLevel
        Dim WarningLevel As Integer = My.Settings.WarningLevel
        bmp = New Bitmap(PictureBox1.Width, PictureBox1.Height)
        gra = Graphics.FromImage(bmp)
        gra.FillRectangle(b, 0, 0, PictureBox1.Width, PictureBox1.Height)
        Dim NoticeStart As Integer = PictureBox1.Width * TrackBar1.Value / 100
        gra.FillRectangle(b1, NoticeStart, 0, PictureBox1.Width - NoticeStart, PictureBox1.Height)
        Dim WarningStart As Integer = PictureBox1.Width * TrackBar2.Value / 100
        gra.FillRectangle(b2, WarningStart, 0, PictureBox1.Width - WarningStart, PictureBox1.Height)
        PictureBox1.Image = bmp
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Dispose()
    End Sub

    Private Sub Save_Button_Click(sender As Object, e As EventArgs) Handles Save_Button.Click
        My.Settings.NoticeLevel = TrackBar1.Value
        My.Settings.WarningLevel = TrackBar2.Value
        If PictureBox2.BackColor <> PictureBox3.BackColor Then
            My.Settings.ThemeColor = PictureBox2.BackColor
            My.Settings.TForeColor = PictureBox3.BackColor
        Else
            MsgBox("主题色不能与字体颜色相同，请重新选择", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        My.Settings.RemindInterval = ComboBox1.Text
        My.Settings.DecreaseAnimation = CheckBox3.Checked
        My.Settings.Save()
        '------开机启动选项----------
        If (CheckBox1.Checked) AndAlso (Not My.Settings.RunWhenSysStart) Then
            Dim Reg As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
            Reg.SetValue(Application.ProductName, Application.StartupPath & "\" & Application.ProductName & ".exe" & " -h") '写入注册表
            Reg.Close()
            My.Settings.RunWhenSysStart = True
        ElseIf (CheckBox1.Checked = False) AndAlso (My.Settings.RunWhenSysStart) Then
            My.Settings.RunWhenSysStart = False
            Dim Reg As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
            Reg.DeleteValue(Application.ProductName) '删除注册表键
            Reg.Close()
        End If
        My.Settings.HideWhenAutoRun = CheckBox2.Checked
        '----------------------------
        My.Settings.Save()
        MsgBox("保存成功！部分设置需要在重启软件后生效", MsgBoxStyle.Information)
        Close()
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        DrawProgressBar()
    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        DrawProgressBar()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        My.Settings.Reset()
        Dim Reg As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
        Reg.SetValue(Application.ProductName, Application.StartupPath & "\" & Application.ProductName & ".exe" & " -h") '写入注册表
        Reg.Close()
        My.Settings.FirstRun = False
        Close()
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Dim r As DialogResult
        r = ColorDialog1.ShowDialog()
        If r = DialogResult.OK Then
            PictureBox2.BackColor = ColorDialog1.Color
        End If
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Dim r As DialogResult
        r = ColorDialog1.ShowDialog()
        If r = DialogResult.OK Then
            PictureBox3.BackColor = ColorDialog1.Color
        End If
    End Sub

    Private Sub Button_About_Click(sender As Object, e As EventArgs) Handles Button_About.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub Button_Help_Click(sender As Object, e As EventArgs) Handles Button_Help.Click
        SendKeys.Send("{F1}")
    End Sub
End Class