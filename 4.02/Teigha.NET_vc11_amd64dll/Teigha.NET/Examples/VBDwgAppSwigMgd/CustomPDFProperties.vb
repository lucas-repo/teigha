Imports System.Windows.Forms

Public Class CustomPDFProperties
    Public vectorRes As UInt16 = 600
    Public hatchRes As UInt16 = 150
    Public imagesRes As UInt16 = 400
    Public monochromeRes As UInt16 = 400

    Public bCropInvisible As Boolean = True
    Public bAdditionalCompression As Boolean = True

    Public imageQ As UInt16 = 50

    Public mergeCtrl As UInt16 = 1
    Public pdfaCtrl As UInt16 = 0

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        vectorResolution.Text = vectorRes.ToString()
        hatchResolution.Text = hatchRes.ToString()
        imagesResolution.Text = imagesRes.ToString()
        monochromeResolution.Text = monochromeRes.ToString()

        chbCropInvisible.Checked = bCropInvisible
        chbImageCompression.Checked = bAdditionalCompression

        imageQuality.Value = imageQ

        mergeControl.SelectedIndex = mergeCtrl
        pdfControl.SelectedIndex = pdfaCtrl

    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        vectorRes = Convert.ToUInt16(vectorResolution.Text)
        hatchRes = Convert.ToUInt16(hatchResolution.Text)
        imagesRes = Convert.ToUInt16(imagesResolution.Text)
        monochromeRes = Convert.ToUInt16(monochromeResolution.Text)

        bCropInvisible = chbCropInvisible.Checked
        bAdditionalCompression = chbImageCompression.Checked

        imageQ = Convert.ToUInt16(imageQuality.Value)

        mergeCtrl = mergeControl.SelectedIndex
        pdfaCtrl = pdfControl.SelectedIndex

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
