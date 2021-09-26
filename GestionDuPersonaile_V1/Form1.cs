using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GestionDuPersonaile_V1.Objets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



namespace GestionDuPersonaile_V1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        #region 7erek l forme !
        private bool Drag;
        private int MouseX;
        private int MouseY;

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();            //Shadow
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT) m.Result = (IntPtr)HTCAPTION;
        }
        private void PanelMove_MouseDown(object sender, MouseEventArgs e)
        {
            Drag = true;
            MouseX = Cursor.Position.X - this.Left;
            MouseY = Cursor.Position.Y - this.Top;
        }
        private void PanelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (Drag)
            {
                this.Top = Cursor.Position.Y - MouseY;
                this.Left = Cursor.Position.X - MouseX;
            }
        }
        private void PanelMove_MouseUp(object sender, MouseEventArgs e) { Drag = false; }
        #endregion

        #region Code pour forcer la saisie de int ou string dans une textBox !
        // Code pour forcer la saisie de int ou string dans une textBox !

        public void verif_entir(KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
                e.Handled = false;
            else if (char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }
        public void verif_char(KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
                e.Handled = true;
            else if (char.IsControl(e.KeyChar))
                e.Handled = true;
            else
                e.Handled = false;
        }
        //============================================================

        private void textBoxEtudFrai_KeyPress(object sender, KeyPressEventArgs e)
        {
            verif_entir(e);
        }

        private void textBoxEtudTel_KeyPress(object sender, KeyPressEventArgs e)
        {
            verif_entir(e);
            if (textBoxEtudTel.Text.Length > 8)
            {
                textBoxEtudMassar.Focus();
            }

        }
        private void textBoxEtudNom_KeyPress(object sender, KeyPressEventArgs e)
        {
            verif_char(e);
        }

        private void textBoxEtudPrenom_KeyPress(object sender, KeyPressEventArgs e)
        {
            verif_char(e);
        }

        private void textBoxEtudNomPer_KeyPress(object sender, KeyPressEventArgs e)
        {
            verif_char(e);
        }

        private void textBoxProfNom_KeyPress(object sender, KeyPressEventArgs e)
        {
            verif_char(e);
        }

        private void textBoxProfPrenom_KeyPress(object sender, KeyPressEventArgs e)
        {
            verif_char(e);
        }






        #endregion

        #region PlaceHolder des textbox de connexion
        private void textBoxUser_Enter(object sender, EventArgs e)
        {
            if (textBoxUser.Text == "User Name")
            {
                textBoxUser.ForeColor = Color.Black;
                textBoxUser.Text = "";
            }
        }

        private void textBoxUser_Leave(object sender, EventArgs e)
        {
            if (textBoxUser.Text == "")
            {
                textBoxUser.ForeColor = Color.Silver;
                textBoxUser.Text = "User Name";
            }
        }

        private void textBoxPassword_Enter(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "Mot de passe")
            {
                textBoxPassword.ForeColor = Color.Black;
                textBoxPassword.Text = "";
            }
        }

        private void textBoxPassword_Leave(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "")
            {
                textBoxPassword.ForeColor = Color.Silver;
                textBoxPassword.Text = "Mot de passe";
            }
        }
        #endregion

        #region focus des textbox Add Prof + (focus qui valide la creation du proff , a revoire !!!!)




        //=================================================================================
        //============= |  Cree nouveau prof  | ====================== 

        private void textBoxProfNom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxProfPrenom.Focus();
        }

        private void textBoxProfPrenom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxCIN.Focus();
        }

        private void textBoxCIN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxProfGSM.Focus();
        }

        private void textBoxProfGSM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxProfMail.Focus();
        }

        private void textBoxProfMail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                comboBox3.Focus();
        }

        private void comboBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxProfAdress.Focus();
        }

        private void textBoxProfAdress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                comboBox1.Focus();
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                dateTimePicker1.Focus();
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                comboBox2.Focus();
        }

        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxSalMois.Focus();
                radioButton1.Checked = true;
            }
        }

        private void textBoxSalMois_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                buttonValide.Focus();

                bool valid = true; // on verifie si tout les champs on été remplis 
                if (textBoxProfNom.Text == "")
                {
                    textBoxProfNom.BackColor = Color.Red;
                    valid = false;
                }
                if (textBoxProfPrenom.Text == "")
                {
                    textBoxProfPrenom.BackColor = Color.Red;
                    valid = false;
                }
                if (textBoxCIN.Text == "")
                {
                    textBoxCIN.BackColor = Color.Red;// on verifie si tout les champs on été remplis 
                    valid = false;
                }
                if (textBoxProfGSM.Text == "" || textBoxProfGSM.Text.Length != 10 || textBoxProfGSM.Text.Substring(0, 2) != "06")
                {
                    textBoxProfGSM.BackColor = Color.Red;
                    valid = false;

                }
                if (textBoxProfMail.Text == "" || comboBox3.SelectedIndex == 0)
                {
                    textBoxProfMail.BackColor = Color.Red;
                    comboBox3.BackColor = Color.Red;
                    valid = false;
                }
                if (textBoxProfAdress.Text == "" || textBoxProfAdress.Text.Length < 10)// on verifie si tout les champs on été remplis 
                {
                    textBoxProfAdress.BackColor = Color.Red;
                    valid = false;
                }
                if (comboBox1.SelectedIndex == 0)
                {
                    comboBox1.BackColor = Color.Red;
                    valid = false;
                }
                int a = int.Parse(textBoxSonAge.Text);
                int b = int.Parse(textBoxSonAge.Text);// on verifie si tout les champs on été remplis 
                if (a < 21 || b > 60)
                {
                    textBoxSonAge.BackColor = Color.Red;
                    valid = false;
                }
                if (comboBox2.SelectedIndex == 0)
                {
                    comboBox2.BackColor = Color.Red;
                    valid = false;
                }
                if (textBoxDurreContrat.Text == "")
                {
                    textBoxDurreContrat.BackColor = Color.Red;
                    valid = false;
                }
                if (textBoxSalMois.Text == "")
                {
                    textBoxSalMois.BackColor = Color.Red;
                    valid = false;
                }


                if (valid == true) // tout est bien dans sa place !
                {

                    string ID = "temp";   //=textBoxID.Text; //  =============================================================< < < < < <<<<                 en attente de code + url de img
                    string Nom = textBoxProfNom.Text;
                    string Prenom = textBoxProfPrenom.Text;
                    string CIN = textBoxCIN.Text;
                    int Tel = int.Parse(textBoxProfGSM.Text);
                    string mail = textBoxProfMail.Text + "@" + comboBox3.SelectedItem;
                    string Adress = textBoxProfAdress.Text;
                    string Niveau = comboBox1.SelectedItem.ToString();
                    DateTime DateNaissance = dateTimePicker1.Value;
                    string matier = comboBox2.SelectedItem.ToString();
                    int Durrer = int.Parse(textBoxDurreContrat.Text);
                    int Salaire = int.Parse(textBoxSalMois.Text);



                    Prof Prof = new Prof(ID, Nom, Prenom, CIN, Tel, mail, Adress, Niveau, DateNaissance, matier, Durrer, Salaire, pictureBox3.Image, true, DateTime.Today, DateTime.Today);
                    ProfList.Add(Prof);
                    MessageBox.Show("validé", "tan tan a été ajouté", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("4alta chi 7aja  !!", "HAHAHA", MessageBoxButtons.OK, MessageBoxIcon.Error); // on as trouvé des champs vide !
                    textBoxProfNom.BackColor = Color.White;
                    textBoxProfPrenom.BackColor = Color.White;
                    textBoxCIN.BackColor = Color.White;
                    textBoxProfGSM.BackColor = Color.White;
                    textBoxProfMail.BackColor = Color.White;
                    textBoxProfAdress.BackColor = Color.White;
                    comboBox1.BackColor = Color.White;
                    dateTimePicker1.BackColor = Color.White;
                    comboBox2.BackColor = Color.White;
                    textBoxDurreContrat.BackColor = Color.White;
                    textBoxSalMois.BackColor = Color.White;
                    textBoxSonAge.BackColor = Color.White;
                    comboBox3.BackColor = Color.White;

                }

            }
        }


        //=================================================================================
        //============= |  Login  | ====================== 
        private void textBoxUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxPassword.Focus();
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bool valid = false;
                foreach (User x in userList)
                {
                    if (textBoxUser.Text == x.Username && textBoxPassword.Text == x.Password)
                    {
                        valid = true;
                        if (x.type == 1)
                        {
                            Grade = x.Grade; // le grade va determiner par la suite si le user est un admin ou une secretaire !
                            panelLogin.Hide();
                            panelBody.Show();
                            panelEditor.Hide();
                            panelEditProf.Hide();
                            panelAfficherPersonel.Hide();
                            panelAddUser.Hide();
                            panelAfficherPersonel.Hide();
                            panelMotdepassOublier.Hide();
                            panelNVmdp.Hide();
                            panelComande.Show();
                            panelChoixList.Hide();
                            panelAfficherPersonel.Hide();
                            label2.Text = "Bienvenue Monsieur ";
                        }
                        if (x.type == 2)
                        {
                            Grade = x.Grade;
                            panelLogin.Hide();
                            panelBody.Show();
                            label2.Text = "Bienvenue Madame ";
                        }
                        label2.Text += textBoxUser.Text;
                    }

                }
                if (valid == false) // si votre login MDP n'est pas trouvé dans la Userlist
                {
                    textBoxUser.Text = textBoxPassword.Text = "";
                    textBoxUser.BackColor = Color.Red;
                    textBoxPassword.BackColor = Color.Red;
                    MessageBox.Show("Le nom ou le mot de pass est incorect !", "Ereur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxUser.BackColor = Color.White;
                    textBoxPassword.BackColor = Color.White;
                    textBoxUser.Text = "User Name";
                    textBoxUser.ForeColor = Color.Silver;
                    textBoxPassword.ForeColor = Color.Silver;
                    textBoxPassword.Text = "Mot de passe";
                }
            }

        }

        //======================================================================================

        private void textBoxMDP1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxMDP2.Focus();
        }

        private void textBoxMDP2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                foreach (User x in userList)
                {
                    if (x.Username == textBoxUser.Text)
                    {
                        if (textBoxMDP1.Text == textBoxMDP2.Text)
                        {
                            x.Password = textBoxMDP1.Text;
                            MessageBox.Show("Votre mot de passe a été modifier avec succés", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBoxMDP1.Text = textBoxMDP2.Text = "";
                            textBoxUser.Text = "User Name";
                            textBoxUser.ForeColor = Color.Silver;
                            panelLogin.Show();
                            panelBody.Show();
                            panelEditor.Hide();
                            panelEditProf.Hide();
                            panelAfficherPersonel.Hide();
                            panelAddUser.Hide();
                            panelAfficherPersonel.Hide();
                            panelMotdepassOublier.Hide();
                            panelNVmdp.Hide();
                            panelComande.Hide();
                            panelChoixList.Hide();
                        }
                        else
                        {
                            textBoxMDP1.Text = textBoxMDP2.Text = "";
                            MessageBox.Show("Veilliez resaisire le mot de passe", "Ereur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                    }
                }
            }
        }
        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                foreach (User x in userList)
                {
                    if (x.Username == textBoxUser.Text)
                    {
                        if (textBox6.Text == x.question)
                        {
                            panelLogin.Hide();
                            panelBody.Show();
                            panelEditor.Hide();
                            panelEditProf.Hide();
                            panelAfficherPersonel.Hide();
                            panelAddUser.Hide();
                            panelAfficherPersonel.Hide();
                            panelMotdepassOublier.Hide();
                            panelNVmdp.Show();
                            panelComande.Hide();
                            panelChoixList.Hide();

                        }
                        else
                        {
                            textBox6.BackColor = Color.Red;
                            MessageBox.Show("Votre reponces est incorrecte !", "Ereur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            MessageBox.Show("Attendez l'arivé d'un administrateur OP", "Ereur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            x.tentative = false;
                            panelLogin.Show();
                            panelBody.Show();
                            panelMotdepassOublier.Hide();
                            textBoxUser.Text = "User Name";
                            textBoxUser.ForeColor = Color.Silver;
                        }
                    }
                }
            }
        }

        //========================== Ajouter Etudiant ========================

        private void textBoxEtudNom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxEtudPrenom.Focus();
        }

        private void textBoxEtudPrenom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxEtudNomPer.Focus();
        }

        private void textBoxEtudNomPer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxEtudTel.Focus();
        }

        private void textBoxEtudTel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxEtudMassar.Focus();
        }

        private void textBoxEtudMassar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxEtudEmail.Focus();
        }

        private void textBoxEtudEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                comboBox6.Focus();
        }

        private void comboBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                comboBox5.Focus();
        }

        private void comboBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                dateTimePicker2.Focus();
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBoxEtudAdress.Focus();
        }

        private void textBoxEtudAdress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonEtudValide.Focus();
        }


        //======================================================================



















        #endregion

        User userAdmin = new User();
        List<User> userList = new List<User>(); //Liste des utilisateur de l'application


        List<Prof> ProfList = new List<Prof>();
        List<Etudiant> EtudiantList = new List<Etudiant>();


        Image DefaultImageProf = Image.FromFile(@"imgs\icons_IMG_Prof.png");

        int Grade; // determine si l'itulisateur est un admine ou un proffe*

        private void Form1_Load(object sender, EventArgs e)
        {
            userList.Add(userAdmin); // connexion aprouvé (temp)

            FileStream f_Etud = File.OpenRead(@"DATA/EtudiantList.bin");
            BinaryFormatter bf_Etud = new BinaryFormatter();
            EtudiantList = (List<Etudiant>)bf_Etud.Deserialize(f_Etud);
            f_Etud.Close();


            FileStream f_prof = File.OpenRead(@"DATA/profList.bin");
            BinaryFormatter bf_prof = new BinaryFormatter();
            ProfList = (List<Prof>)bf_prof.Deserialize(f_prof);
            f_prof.Close();



            DateTime date = DateTime.Today;
            bool up = false;
            if (date.Month == 9 && date.Day == 1) // les etudiant monte de niveau une fois , le premier jours du mois 9
            {
                if (up == false)
                {
                    up = true;
                    foreach (Etudiant x in EtudiantList)
                    {
                        if (x.Niveau < 6 && x.ex == true)
                            x.Niveau++;
                        if (x.Niveau == 7)// si il dépasse CM3 il est suprimer de la liste principale
                            x.ex = false;
                    }
                    f_Etud = File.Create(@"DATA/EtudiantList.bin");
                    bf_Etud = new BinaryFormatter();
                    bf_Etud.Serialize(f_Etud, EtudiantList);
                    f_Etud.Close();
                }
            }










            toolTipAcceil.SetToolTip(buttonAcceille, "Acceuil");
            toolTipLists.SetToolTip(buttonAfficher, "Listes des membre");
            toolTipAdd.SetToolTip(buttonModifier, "Ajouter/Modifier un membre");
            toolTipCV.SetToolTip(buttonCV, "Listes des CV en attente");
            toolTipNotes.SetToolTip(buttonNotes, "Notes des éleves");
            toolTipBackUp.SetToolTip(buttonBackUp, "Back UP");

            buttonEtudSup.Hide();
            buttonEtudModifier.Hide();
            buttonEtudAplik.Hide();



            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;// combobox de la forme de "Ajouter Prof"
            comboBox3.SelectedIndex = 0;

            comboBox6.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;// combobox de la forme de "Ajouter Etudiants"


            textBoxUser.ForeColor = Color.Silver;//initialisation du contenue textbox de Login-MDP                                       //  ===========   ici
            textBoxUser.Text = "User Name";
            textBoxPassword.ForeColor = Color.Silver;
            textBoxPassword.Text = "Mot de passe";


            //=============================================================================================================   Ne pas oublier !!!!!!!!!!!!!
            panelLogin.Show();
            panelBody.Show();
            panelEditor.Hide();
            panelEditProf.Hide();
            panelAfficherPersonel.Hide();
            panelAddUser.Hide();
            panelAfficherPersonel.Hide();
            panelMotdepassOublier.Hide();
            panelChoixList.Hide();
            panelNVmdp.Hide();
            panelComande.Hide();
            panelChoixList.Hide();
            panelEditEtudiant.Hide();
            panelListEtudiant.Hide();
            panelEditEtudiant.Hide();
            panelChoixLsSupp.Hide();
            panelLsExProf.Hide();
            panelLsEtudSupp.Hide();
        }                                                                                                                                // ============= ici


        private void button3_Click(object sender, EventArgs e)// Bouton ajouter proff
        {
            panelLogin.Hide();
            panelBody.Show();
            panelEditor.Hide();
            panelEditProf.Show();
            panelChoixList.Hide();
            panelAddUser.Hide();
            panelAfficherPersonel.Hide();
            panelMotdepassOublier.Hide();
            panelNVmdp.Hide();
            panelComande.Show();
            panelMotdepassOublier.Hide();
            panelListEtudiant.Hide();
            panelEditEtudiant.Hide();
            panelChoixLsSupp.Hide();
            panelLsExProf.Hide();
            buttonProfApli.Hide();
            panelLsEtudSupp.Hide();
            dateTimePicker1.Value = DateTime.Today;
            textBoxProfNom.Text = "";
            textBoxProfPrenom.Text = "";
            textBoxCIN.Text = "";
            textBoxProfGSM.Text = "";
            textBoxProfMail.Text = "";
            comboBox3.Text = "";
            textBoxProfAdress.Text = "";
            comboBox1.SelectedIndex = 0;
            textBoxSonAge.Text = "";
            comboBox2.SelectedIndex = 0;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            textBoxSalMois.Text = "";

            dateTimePicker1.Enabled = true;
            textBoxProfNom.Enabled = true;
            textBoxProfPrenom.Enabled = true;
            textBoxCIN.Enabled = true;
            textBoxProfGSM.Enabled = true;
            textBoxProfMail.Enabled = true;
            comboBox3.Enabled = true;
            textBoxProfAdress.Enabled = true;
            comboBox1.Enabled = true;

            comboBox2.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            textBoxSalMois.Enabled = true;

            pictureBox3.Image = DefaultImageProf;


            string ID = "";
            int i = 0;
            int a = 0;
            string temp = "";
            if (ProfList == null || ProfList.Count == 0)
            {
                ID = "P000001";
            }
            else
            {
                foreach (Prof E in ProfList)
                {
                    i++;
                    temp = E.ID.Split('P')[1];
                    a = int.Parse(temp);
                    if (a != i)
                    {
                        ID = string.Format("{0 ,0:D6}", i);
                        ID = "P" + ID;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(ID))
                {
                    ID = string.Format("{0 ,0:D6}", i + 1);
                    ID = "P" + ID;
                }
            }


            textBoxID.Text = ID;
        }



        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button2_Click_2(object sender, EventArgs e)
        {
            panelLogin.Hide();
            panelEditEtudiant.Hide();
            panelBody.Show();
            panelEditor.Hide();
            panelEditProf.Hide();
            panelAfficherPersonel.Hide();
            panelAddUser.Hide();
            panelChoixList.Hide();
            panelAfficherPersonel.Hide();
            panelMotdepassOublier.Hide();
            panelNVmdp.Hide();
            panelComande.Show();
            panelMotdepassOublier.Hide();
            panelChoixLsSupp.Hide();
            panelLsExProf.Hide();
            panelListEtudiant.Hide();
            panelLsEtudSupp.Hide();
        }
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            bool valid = false;
            foreach (User x in userList)
            {
                if (textBoxUser.Text == x.Username && textBoxPassword.Text == x.Password)
                {
                    valid = true;
                    if (x.type == 1)
                    {
                        Grade = x.Grade; // le grade va determiner par la suite si le user est un admin ou une secretaire !
                        panelLogin.Hide();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Hide();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelAfficherPersonel.Hide();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Hide();
                        panelChoixList.Hide();
                        panelComande.Show();
                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        panelMotdepassOublier.Hide();
                        panelEditEtudiant.Hide();
                        panelListEtudiant.Hide();
                        panelLsEtudSupp.Hide();
                        label2.Text = "Bienvenue Monsieur ";
                    }
                    if (x.type == 2)
                    {
                        Grade = x.Grade;
                        panelLogin.Hide();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Hide();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelAfficherPersonel.Hide();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Hide();
                        panelComande.Show();
                        panelChoixList.Hide();
                        panelMotdepassOublier.Hide();
                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        panelEditEtudiant.Hide();
                        panelListEtudiant.Hide();
                        panelLsEtudSupp.Hide();
                        label2.Text = "Bienvenue Madame ";
                    }
                    label2.Text += textBoxUser.Text;

                }

            }
            if (valid == false) // si votre login MDP n'est pas trouvé dans la Userlist
            {
                textBoxUser.Text = textBoxPassword.Text = "";
                textBoxUser.BackColor = Color.Red;
                textBoxPassword.BackColor = Color.Red;
                MessageBox.Show("Le nom ou le mot de pass est incorect !", "Ereur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxUser.BackColor = Color.White;
                textBoxPassword.BackColor = Color.White;
                textBoxUser.Text = "User Name";
                textBoxUser.ForeColor = Color.Silver;
                textBoxPassword.ForeColor = Color.Silver;
                textBoxPassword.Text = "Mot de passe";
            }

        }


        private void buttonModifier_Click(object sender, EventArgs e)
        {
            panelLogin.Hide();
            panelBody.Show();
            panelEditor.Show();
            panelEditProf.Hide();
            panelAfficherPersonel.Hide();
            panelAddUser.Hide();
            panelChoixList.Hide();
            panelMotdepassOublier.Hide();
            panelNVmdp.Hide();
            panelComande.Show();
            panelMotdepassOublier.Hide();
            panelEditEtudiant.Hide();
            panelListEtudiant.Hide();
            panelChoixLsSupp.Hide();
            panelLsExProf.Hide();
            panelLsEtudSupp.Hide();
        }



        // hover apliqué au panel , l'ors du choix d'ajouter un prof ou un etudiant !=======
        private void button3_MouseEnter(object sender, EventArgs e)
        {

            panelEditor.BackColor = Color.DarkCyan;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            panelEditor.BackColor = Color.Gainsboro;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            panelEditor.BackColor = Color.Green;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            panelEditor.BackColor = Color.Gainsboro;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // < < < < < <  <====================================================================================== < < < < < <   en attente "ajouter etudiant"

            panelEditEtudiant.Show();
            panelLogin.Hide();
            panelBody.Show();
            panelEditor.Hide();
            panelEditProf.Hide();
            panelAfficherPersonel.Hide();
            panelAddUser.Hide();
            panelAfficherPersonel.Hide();
            panelMotdepassOublier.Hide();
            panelNVmdp.Hide();
            panelComande.Show();
            panelChoixList.Hide();
            panelListEtudiant.Hide();
            panelChoixLsSupp.Hide();
            panelLsExProf.Hide();
            panelLsEtudSupp.Hide();
            textBoxEtudNom.Enabled = true;
            textBoxEtudPrenom.Enabled = true;
            textBoxEtudNomPer.Enabled = true;
            textBoxEtudTel.Enabled = true;
            textBoxEtudMassar.Enabled = true;
            textBoxEtudEmail.Enabled = true;
            comboBox5.Enabled = true;
            comboBox6.Enabled = true;
            radioButton5.Enabled = true;
            radioButton6.Enabled = true;
            radioButton7.Enabled = true;
            radioButton8.Enabled = true;
            radioButton9.Enabled = true;
            radioButton10.Enabled = true;
            radioButton11.Enabled = true;
            textBoxEtudAge.Enabled = true;
            textBoxEtudAdress.Enabled = true;
            buttonEtudAplik.Enabled = true;
            dateTimePicker2.Enabled = true;


            //buttonEtudAplik.Hide();
            //buttonEtudModifier.Enabled = false;
            //buttonEtudSup.Enabled = false;

            pictureBox4.Image = DefaultImageProf;
            textBoxEtudNom.Text = "";
            textBoxEtudPrenom.Text = "";
            textBoxEtudNomPer.Text = "";
            textBoxEtudTel.Text = "";
            textBoxEtudMassar.Text = "";
            textBoxEtudFrai.Text = textBoxEtudEmail.Text = "";
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            radioButton5.Checked = radioButton6.Checked = radioButton7.Checked = radioButton8.Checked = radioButton9.Checked = radioButton10.Checked = radioButton11.Checked = false;
            textBoxEtudAge.Text = "0";
            textBoxEtudAdress.Text = "";

            dataGridView2.Rows.Clear();
            foreach (Etudiant x in EtudiantList)
            {
                dataGridView2.Rows.Add(x.ID, x.Nom, x.Prenom, x.Niveau, x.telephone, x.NomTuteur, x.Email, x.autre);
            }

            string ID = "";
            int i = 0;
            int a = 0;
            string temp = "";
            if (EtudiantList == null || EtudiantList.Count == 0)
            {
                ID = "E000001";
            }
            else
            {
                foreach (Etudiant E in EtudiantList)
                {
                    i++;
                    temp = E.ID.Split('E')[1];
                    a = int.Parse(temp);
                    if (a != i)
                    {
                        ID = string.Format("{0 ,0:D6}", i);
                        ID = "E" + ID;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(ID))
                {
                    ID = string.Format("{0 ,0:D6}", i + 1);
                    ID = "E" + ID;
                }
            }


            textBoxEtudID.Text = ID;



        }







        private void textBox5_TextChanged(object sender, EventArgs e) // textbox du salaire par mois !!!!!
        {

            if (textBoxSalMois.Text == "" || textBoxDurreContrat.Text == "" && radioButton1.Checked == false && radioButton2.Checked == false)
            {
                radioButton1.Checked = true;
                textBoxDurreContrat.Text = "2";
                textBoxSalAnuel.Text = "";
                textBoxSalCumul.Text = "";
            }
            else
            {
                double x = double.Parse(textBoxSalMois.Text);
                textBoxSalAnuel.Text = Convert.ToString(x * 12);
                textBoxSalCumul.Text = Convert.ToString(double.Parse(textBoxSalAnuel.Text) * int.Parse(textBoxDurreContrat.Text));
            }






            // Calcule automatique du salaire anuel du proff !!!!!
            //if (textBoxSalMois.Text == "" || textBoxDurreContrat.Text == "")
            //{
            //    textBoxSalAnuel.Text = "";
            //    textBoxDurreContrat.Text = "2";
            //}
            //else
            //{
            //    textBoxSalAnuel.Text = (int.Parse(textBoxSalMois.Text) * 12).ToString();     //crash de string dans int "user erorr" < RESOLU >
            //}                                                                               //
            //
        }                                                                                 //
        private void textBoxSalMois_KeyPress(object sender, KeyPressEventArgs e)         //
        {                                                                               //
            verif_entir(e);                                                            // Verifié !!!
        }


        private void textBoxSalAnuel_TextChanged(object sender, EventArgs e)
        {
            //if (textBoxSalAnuel.Text == "" && textBoxDurreContrat.Text=="" && (radioButton2.Checked==false && radioButton1.Checked==false))
            //{
            //    textBoxSalCumul.Text = "";
            //    textBoxSalAnuel.Text = "";
            //}
            //else
            //{
            //    int s = 0;
            //    int a = int.Parse(textBoxDurreContrat.Text);
            //    int b = int.Parse(textBoxSalAnuel.Text);
            //    s = a * b;
            //    textBoxSalCumul.Text = s.ToString();
            //}
        }



        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)  // LinkLabel : MDP oublier!
        {



            if (textBoxUser.Text == "" || textBoxUser.Text == "User Name")
                MessageBox.Show("Veillez remplire le nom d'utilisateur", "ereur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                foreach (User x in userList)
                {
                    if (x.Username == textBoxUser.Text)
                    {
                        if (x.tentative == true)
                        {
                            textBox6.Text = "";
                            textBoxMDP1.Text = textBoxMDP2.Text = "";
                            panelLogin.Hide();
                            panelBody.Show();
                            panelEditor.Hide();
                            panelEditProf.Hide();
                            panelAfficherPersonel.Hide();
                            panelAddUser.Hide();
                            panelAfficherPersonel.Hide();
                            panelMotdepassOublier.Show();
                            panelNVmdp.Hide();
                            panelComande.Hide();
                            panelChoixList.Hide();
                            panelEditEtudiant.Hide();
                            panelChoixLsSupp.Hide();
                            panelLsExProf.Hide();
                            panelListEtudiant.Hide();
                            panelLsEtudSupp.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Nous ne pouvont pas verifier votre identité", "ereur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                foreach (User x in userList)
                {
                    if (x.Username == textBoxUser.Text)
                    {
                        label24.Text = "Question " + x.position.ToString();

                    }
                }
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {

            foreach (Prof x in ProfList)
            {
                if (x.ex == true)
                {
                    if (x.ID == textBoxID.Text)
                    {
                        x.ex = false;
                        x.fin = DateTime.Today;
                        panelLogin.Hide();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Hide();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelAfficherPersonel.Hide();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Hide();
                        panelComande.Show();
                        panelChoixList.Show();
                        panelEditEtudiant.Hide();
                        panelListEtudiant.Hide();
                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        panelLsEtudSupp.Hide();

                    }
                }
                else
                {
                    if (MessageBox.Show("Cette action est irreversible !", "Attention !", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        ProfList.Remove(x);
                        panelLogin.Hide();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Hide();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelAfficherPersonel.Hide();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Hide();
                        panelComande.Show();
                        panelChoixList.Show();
                        panelEditEtudiant.Hide();
                        panelListEtudiant.Hide();
                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        panelLsEtudSupp.Hide();
                        break;
                    }
                }
            }
            FileStream f_prof = File.Create(@"DATA/profList.bin");
            BinaryFormatter bf_prof = new BinaryFormatter();
            bf_prof.Serialize(f_prof, ProfList);
            f_prof.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                textBoxDurreContrat.Text = "2";
            }
            else
            {
                textBoxDurreContrat.Text = "25";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                textBoxDurreContrat.Text = "25";
            }
            else
            {
                textBoxDurreContrat.Text = "2";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

            if (textBoxSalMois.Text == "" || textBoxDurreContrat.Text == "" && radioButton1.Checked == false && radioButton2.Checked == false)
            {
                radioButton1.Checked = true;
                textBoxDurreContrat.Text = "2";
                textBoxSalAnuel.Text = "";
                textBoxSalCumul.Text = "";
            }
            else
            {
                double x = double.Parse(textBoxSalMois.Text);
                textBoxSalAnuel.Text = Convert.ToString(x * 12);
                textBoxSalCumul.Text = Convert.ToString(double.Parse(textBoxSalAnuel.Text) * int.Parse(textBoxDurreContrat.Text));
            }




            //if (textBoxSalAnuel.Text == "")
            //{
            //    textBoxSalCumul.Text = "";
            //}
            //else
            //{
            //    int s = 0;
            //    int a = int.Parse(textBoxDurreContrat.Text);
            //    int b = int.Parse(textBoxSalAnuel.Text);
            //    s = a * b;
            //    textBoxSalCumul.Text = s.ToString();
            //}
            //if (textBoxSalMois.Text == "" || textBoxDurreContrat.Text == "")
            //{
            //    textBoxSalAnuel.Text = "";
            //    textBoxDurreContrat.Text = "";
            //}
            //else
            //{
            //    textBoxSalAnuel.Text = (int.Parse(textBoxSalMois.Text) * 12).ToString();    
            //} 

        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) // calcule de l'age du prof
        {
            textBoxSonAge.Text = (DateTime.Now.Year - dateTimePicker1.Value.Year).ToString();
        }



        private void buttonValide_Click(object sender, EventArgs e)
        {
            bool valid = true; // on verifie si tout les champs on été remplis 
            if (textBoxProfNom.Text == "")
            {
                textBoxProfNom.BackColor = Color.Red;
                valid = false;
            }
            if (textBoxProfPrenom.Text == "")
            {
                textBoxProfPrenom.BackColor = Color.Red;
                valid = false;
            }
            foreach (Prof n in ProfList)
            {

                if (textBoxCIN.Text == "" || n.CIN == textBoxCIN.Text)
                {
                    if (MessageBox.Show("Numéro CIN déja existant , Continuer ?", "OPERATEUR", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                    {
                        textBoxCIN.BackColor = Color.Red;// on verifie si tout les champs on été remplis 
                        valid = false;
                    }
                }
            }
            foreach (Prof p in ProfList)
            {
                if (p.Tel == int.Parse(textBoxProfGSM.Text))
                {
                    if (MessageBox.Show("Numéro GSM déja existant , Continuer ?", "OPERATEUR", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                    {
                        textBoxProfGSM.BackColor = Color.Red;
                        valid = false;
                    }
                }
            }
            if (textBoxProfGSM.Text == "" || textBoxProfGSM.Text.Length != 10 || textBoxProfGSM.Text.Substring(0, 2) != "06")
            {
                textBoxProfGSM.BackColor = Color.Red;
                valid = false;

            }
            if (textBoxProfMail.Text == "" || comboBox3.SelectedIndex == 0)
            {
                textBoxProfMail.BackColor = Color.Red;
                comboBox3.BackColor = Color.Red;
                valid = false;
            }
            if (textBoxProfAdress.Text == "" || textBoxProfAdress.Text.Length < 10)// on verifie si tout les champs on été remplis 
            {
                textBoxProfAdress.BackColor = Color.Red;
                valid = false;
            }
            if (comboBox1.SelectedIndex == 0)
            {
                comboBox1.BackColor = Color.Red;
                valid = false;
            }
            int a = int.Parse(textBoxSonAge.Text);
            int b = int.Parse(textBoxSonAge.Text);// on verifie si tout les champs on été remplis 
            if (a < 21 || b > 60)
            {
                textBoxSonAge.BackColor = Color.Red;
                valid = false;
            }
            if (comboBox2.SelectedIndex == 0)
            {
                comboBox2.BackColor = Color.Red;
                valid = false;
            }
            if (textBoxDurreContrat.Text == "")
            {
                textBoxDurreContrat.BackColor = Color.Red;
                valid = false;
            }
            if (textBoxSalMois.Text == "")
            {
                textBoxSalMois.BackColor = Color.Red;
                valid = false;
            }


            if (valid == true) // tout est bien dans sa place !
            {

                string ID = textBoxID.Text; //  =============================================================< < < < < <<<<                 en attente de code + url de img
                string Nom = textBoxProfNom.Text;
                string Prenom = textBoxProfPrenom.Text;
                string CIN = textBoxCIN.Text;
                int Tel = int.Parse(textBoxProfGSM.Text);
                string mail = textBoxProfMail.Text + "@" + comboBox3.SelectedItem;
                string Adress = textBoxProfAdress.Text;
                string Niveau = comboBox1.SelectedItem.ToString();
                DateTime DateNaissance = dateTimePicker1.Value;
                string matier = comboBox2.SelectedItem.ToString();
                int Durrer = int.Parse(textBoxDurreContrat.Text);
                int Salaire = int.Parse(textBoxSalMois.Text);



                Prof Prof = new Prof(ID, Nom, Prenom, CIN, Tel, mail, Adress, Niveau, DateNaissance, matier, Durrer, Salaire, pictureBox3.Image, true, DateTime.Today, DateTime.Today);
                ProfList.Add(Prof);
                MessageBox.Show("le prof a été ajouté avec sucés", "Operateur", MessageBoxButtons.OK, MessageBoxIcon.Information);

                panelAfficherPersonel.Hide();
                panelEditProf.Hide();
                panelChoixList.Show();



                radioButton1.Checked = radioButton2.Checked = false;
                comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox3.SelectedIndex = 0;
                textBoxProfNom.Text = textBoxID.Text = textBoxProfPrenom.Text = textBoxCIN.Text = textBoxProfMail.Text = textBoxProfAdress.Text = textBoxDurreContrat.Text = textBoxSalMois.Text = "";


                dateTimePicker1.Value = DateTime.Today;
                dateTimePicker1.Value = DateTime.Today;
                textBoxProfNom.Text = "";
                textBoxProfPrenom.Text = "";
                textBoxCIN.Text = "";
                textBoxProfGSM.Text = "";
                textBoxProfMail.Text = "";
                comboBox3.Text = "";
                textBoxProfAdress.Text = "";
                comboBox1.SelectedIndex = 0;
                textBoxSonAge.Text = "";
                comboBox2.SelectedIndex = 0;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                textBoxSalMois.Text = "";





                FileStream f_prof = File.Create(@"DATA/profList.bin");
                BinaryFormatter bf_prof = new BinaryFormatter();
                bf_prof.Serialize(f_prof, ProfList);
                f_prof.Close();





                dataGridView1.Rows.Clear();
                foreach (Prof x in ProfList)
                {
                    dataGridView1.Rows.Add(x.ID, x.Nom, x.Prenom, x.Niveau, x.matier, x.Salaire, x.Durrer);
                }


            }
            else
            {
                MessageBox.Show("Veilliez remplire tout les champs !", "Operateur", MessageBoxButtons.OK, MessageBoxIcon.Error); // on as trouvé des champs vide !
                textBoxProfNom.BackColor = Color.White;
                textBoxProfPrenom.BackColor = Color.White;
                textBoxCIN.BackColor = Color.White;
                textBoxProfGSM.BackColor = Color.White;
                textBoxProfMail.BackColor = Color.White;
                textBoxProfAdress.BackColor = Color.White;
                comboBox1.BackColor = Color.White;
                dateTimePicker1.BackColor = Color.White;
                comboBox2.BackColor = Color.White;
                textBoxDurreContrat.BackColor = Color.White;
                textBoxSalMois.BackColor = Color.White;
                textBoxSonAge.BackColor = Color.White;
                comboBox3.BackColor = Color.White;

            }




        }




        // on as verifier que tout les camps été valide avan de créé le constructeur !





        private void buttonBack_Click(object sender, EventArgs e)
        {
            panelAfficherPersonel.Hide();
            panelEditor.Show();
            panelChoixLsSupp.Hide();
            panelLsExProf.Hide();
            panelLsEtudSupp.Hide();
        }

        private void buttonAfficher_Click(object sender, EventArgs e)
        {
            panelLogin.Hide();
            panelBody.Show();
            panelEditor.Hide();
            panelEditProf.Hide();
            panelAfficherPersonel.Hide();
            panelAddUser.Hide();
            panelAfficherPersonel.Hide();
            panelMotdepassOublier.Hide();
            panelNVmdp.Hide();
            panelComande.Show();
            panelChoixList.Show();
            panelEditEtudiant.Hide();
            panelListEtudiant.Hide();
            panelChoixLsSupp.Hide();
            panelLsExProf.Hide();
            panelLsEtudSupp.Hide();
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            panelBody.Show();
            panelAddUser.Show();
            panelEditor.Hide();
            panelLsEtudSupp.Hide();
        }

        private void button2_Click_3(object sender, EventArgs e)
        {
            int type = 1;
            if (radioButton3.Checked == true)
                type = 1;
            if (radioButton4.Checked == true)
                type = 2;
            User p = new User(textBox2.Text, textBox3.Text, int.Parse(textBox4.Text), type, textBox5.Text, comboBox4.SelectedIndex);
            MessageBox.Show("Votre compte a été créé avec sucsé", "félicitation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
            panelBody.Show();
            panelAddUser.Hide();
            panelEditor.Hide();
            panelChoixList.Hide();
            panelChoixLsSupp.Hide();
            panelLsExProf.Hide();
            panelLsEtudSupp.Hide();
        }

        private void buttonDeconexion_Click(object sender, EventArgs e)
        {
            panelLogin.Show();
            panelBody.Show();
            panelEditor.Hide();
            panelEditProf.Hide();
            panelAfficherPersonel.Hide();
            panelAddUser.Hide();
            panelAfficherPersonel.Hide();
            panelMotdepassOublier.Hide();
            panelNVmdp.Hide();
            panelEditEtudiant.Hide();
            panelChoixList.Hide();
            panelComande.Hide();
            panelListEtudiant.Hide();
            panelChoixLsSupp.Hide();
            panelLsExProf.Hide();
            panelLsEtudSupp.Hide();
            textBoxPassword.Text = "Mot de passe";
            textBoxUser.Text = "User Name";
            textBoxPassword.ForeColor = Color.Silver;
            textBoxUser.ForeColor = Color.Silver;

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            foreach (User x in userList)
            {
                if (x.Username == textBoxUser.Text)
                {
                    if (textBox6.Text == x.question)
                    {
                        panelLogin.Hide();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Hide();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelAfficherPersonel.Hide();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Show();
                        panelComande.Hide();
                        panelChoixList.Hide();
                        panelEditEtudiant.Hide();
                        panelListEtudiant.Hide();
                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        panelLsEtudSupp.Hide();
                    }
                    else
                    {
                        textBox6.BackColor = Color.Red;
                        MessageBox.Show("Votre reponces est incorrecte !", "Ereur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        MessageBox.Show("Attendez l'arivé d'un administrateur OP", "Ereur", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        x.tentative = false;
                        panelLogin.Show();
                        panelBody.Show();
                        panelMotdepassOublier.Hide();
                        textBoxUser.Text = "User Name";
                        textBoxUser.ForeColor = Color.Silver;
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (User x in userList)
            {
                if (x.Username == textBoxUser.Text)
                {
                    if (textBoxMDP1.Text == textBoxMDP2.Text)
                    {
                        x.Password = textBoxMDP1.Text;
                        MessageBox.Show("Votre mot de passe a été modifier avec succés", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBoxMDP1.Text = textBoxMDP2.Text = "";
                        textBoxUser.Text = "User Name";
                        textBoxUser.ForeColor = Color.Silver;
                        panelLogin.Show();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Hide();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelAfficherPersonel.Hide();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Hide();
                        panelComande.Hide();
                        panelChoixList.Hide();
                        panelEditEtudiant.Hide();
                        panelListEtudiant.Hide();
                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        panelLsEtudSupp.Hide();
                    }
                    else
                    {
                        textBoxMDP1.Text = textBoxMDP2.Text = "";
                        MessageBox.Show("Veilliez resaisire le mot de passe", "Ereur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panelChoixLsSupp.Hide();
            panelLsExProf.Hide();
            panelLogin.Hide();
            panelBody.Show();
            panelEditor.Hide();
            panelEditProf.Hide();
            panelAfficherPersonel.Show();
            panelAddUser.Hide();
            panelEditEtudiant.Hide();
            panelMotdepassOublier.Hide();
            panelNVmdp.Hide();
            panelComande.Show();
            panelMotdepassOublier.Hide();
            panelChoixList.Hide();
            panelListEtudiant.Hide();
            panelLsEtudSupp.Hide();
            dataGridView1.Rows.Clear();
            foreach (Prof x in ProfList)
            {
                if (x.ex == true)
                    dataGridView1.Rows.Add(x.ID, x.Nom, x.Prenom, x.Niveau, x.matier, x.Salaire, x.Durrer);


            }
        }

        private void buttonListProf_MouseEnter(object sender, EventArgs e)
        {
            panelChoixList.BackColor = Color.DarkCyan;
        }

        private void buttonListProf_MouseLeave(object sender, EventArgs e)
        {
            panelChoixList.BackColor = Color.Gainsboro;
        }

        private void buttonEtudValide_Click(object sender, EventArgs e)
        {
            bool verife = true;

            foreach (Etudiant x in EtudiantList)
            {
                if (x.CodeMassar == textBoxEtudMassar.Text)
                {
                    textBoxEtudMassar.BackColor = Color.Red;
                    MessageBox.Show("Code Massar existant", "Ereur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    verife = false;
                }
                //if (x.telephone == int.Parse(textBoxEtudTel.Text) && textBoxEtudTel.Text != "")
                //{
                //    textBoxEtudTel.BackColor = Color.Red;
                //    MessageBox.Show("Le numero de telephone existe déja dans la base de donné , voulez vous continuer ?", "Surcharge", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                //}
            }
            if (textBoxEtudTel.Text == "")
            {
                textBoxEtudTel.BackColor = Color.Red;
                verife = false;
            }
            if (int.Parse(textBoxEtudAge.Text) > 12 || int.Parse(textBoxEtudAge.Text) < 5)
            {
                textBoxEtudAge.BackColor = Color.Red;
                verife = false;
            }
            if (radioButton5.Checked == false && radioButton6.Checked == false)
            {
                verife = false;
            }
            if (radioButton7.Checked == false && radioButton8.Checked == false)
            {
                verife = false;
            }
            if (textBoxEtudFrai.Text == "")
            {
                textBoxEtudFrai.BackColor = Color.Red;
                verife = false;
            }
            if (comboBox6.SelectedIndex == 0)
            {
                comboBox6.BackColor = Color.Red;
                verife = false;
            }
            if (comboBox5.SelectedIndex == 0)
            {
                comboBox5.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudNom.Text == "")
            {
                textBoxEtudNom.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudPrenom.Text == "")
            {
                textBoxEtudPrenom.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudNomPer.Text == "")
            {
                textBoxEtudNomPer.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudTel.Text == "" || textBoxEtudTel.Text.Length != 10 || textBoxEtudTel.Text.Substring(0, 2) != "06")
            {
                textBoxEtudTel.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudAdress.Text == "" || textBoxEtudAdress.Text.Length < 10)
            {
                textBoxEtudAdress.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudMassar.Text == "")
            {
                textBoxEtudMassar.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudEmail.Text == "")
            {
                textBoxEtudEmail.BackColor = Color.Red;
                verife = false;
            }
            if (verife == false)
            {
                MessageBox.Show("Des ereur en été déplorer lors de la saisie des donné !", "Ereur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBox5.BackColor = Color.White;
                textBoxEtudAdress.BackColor = Color.White;
                textBoxEtudNomPer.BackColor = Color.White;
                textBoxEtudFrai.BackColor = Color.White;
                comboBox6.BackColor = Color.White;
                textBoxEtudEmail.BackColor = Color.White;
                textBoxEtudAge.BackColor = Color.White;
                textBoxEtudTel.BackColor = Color.White;
                textBoxEtudMassar.BackColor = Color.White;
                textBoxEtudNom.BackColor = Color.White;
                textBoxEtudPrenom.BackColor = Color.White;
            }
            else
            {


                string ID = textBoxEtudID.Text;
                string Nom = textBoxEtudNom.Text;
                string Prenom = textBoxEtudPrenom.Text;
                string tuteur = textBoxEtudNomPer.Text;
                int tel = int.Parse(textBoxEtudTel.Text);
                string massar = textBoxEtudMassar.Text;
                string email = textBoxEtudEmail.Text + "@" + comboBox6.SelectedItem;
                int niveau = comboBox5.SelectedIndex;
                DateTime DateNaissance = dateTimePicker2.Value;
                bool type = radioButton5.Checked;
                string adress = textBoxEtudAdress.Text;
                bool physique = radioButton7.Checked;
                int autre = 0;
                if (radioButton9.Checked == true)
                    autre = 1;
                if (radioButton10.Checked == true)
                    autre = 2;
                if (radioButton11.Checked == true)
                    autre = 3;
                int frai = int.Parse(textBoxEtudFrai.Text);

                Etudiant etudiant = new Etudiant(ID, Nom, Prenom, tuteur, tel, massar, email, niveau, DateNaissance, type, adress, physique, autre, frai, pictureBox4.Image, true);
                EtudiantList.Add(etudiant);
                MessageBox.Show("Données enregistrer avec sucés", "Operateur", MessageBoxButtons.OK, MessageBoxIcon.Information);

                panelLogin.Hide();
                panelBody.Show();
                panelEditor.Hide();
                panelEditProf.Hide();
                panelAfficherPersonel.Hide();
                panelAddUser.Hide();
                panelEditEtudiant.Hide();
                panelMotdepassOublier.Hide();
                panelNVmdp.Hide();
                panelComande.Show();
                panelMotdepassOublier.Hide();
                panelChoixList.Hide();
                panelListEtudiant.Show();
                panelChoixLsSupp.Hide();
                panelLsExProf.Hide();
                panelLsEtudSupp.Hide();
                FileStream f_Etud = File.Create(@"DATA/EtudiantList.bin");
                BinaryFormatter bf_Etud = new BinaryFormatter();
                bf_Etud.Serialize(f_Etud, EtudiantList);
                f_Etud.Close();



                textBoxEtudNom.Text = "";
                textBoxEtudPrenom.Text = "";
                textBoxEtudNomPer.Text = "";
                textBoxEtudTel.Text = "";
                textBoxEtudMassar.Text = "";
                textBoxEtudFrai.Text = textBoxEtudEmail.Text = "";
                comboBox5.SelectedIndex = 0;
                comboBox6.SelectedIndex = 0;
                radioButton5.Checked = radioButton6.Checked = radioButton7.Checked = radioButton8.Checked = radioButton9.Checked = radioButton10.Checked = radioButton11.Checked = false;
                textBoxEtudAge.Text = "0";
                dateTimePicker2.Value = DateTime.Today;


                dataGridView2.Rows.Clear();
                foreach (Etudiant x in EtudiantList)
                {
                    dataGridView2.Rows.Add(x.ID, x.Nom, x.Prenom, x.Niveau, "0" + x.telephone, x.NomTuteur, x.Email, x.autre);
                }





            }






        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                textBoxEtudFrai.Enabled = true;
            else
                textBoxEtudFrai.Enabled = false;
        }

        private void textBoxEtudFrai_TextChanged(object sender, EventArgs e)
        {
            if (textBoxEtudFrai.Text == "")
            { textBox13.Text = textBox14.Text = ""; }
            else
            {
                textBox13.Text = Convert.ToString(double.Parse(textBoxEtudFrai.Text) * 3);
                textBox14.Text = Convert.ToString(double.Parse(textBoxEtudFrai.Text) * 10);
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            textBoxEtudAge.Text = Convert.ToString(DateTime.Now.Year - dateTimePicker2.Value.Year);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex == 0)
                textBoxEtudFrai.Text = "";
            if (comboBox5.SelectedIndex == 1)
                textBoxEtudFrai.Text = "700";
            if (comboBox5.SelectedIndex == 2)
                textBoxEtudFrai.Text = "700";
            if (comboBox5.SelectedIndex == 3) // prix selon les niveaux !!!
                textBoxEtudFrai.Text = "750";
            if (comboBox5.SelectedIndex == 4)
                textBoxEtudFrai.Text = "800";
            if (comboBox5.SelectedIndex == 5)
                textBoxEtudFrai.Text = "850";
            if (comboBox5.SelectedIndex == 6)
                textBoxEtudFrai.Text = "900";

        }

        private void buttonListEtudiant_Click(object sender, EventArgs e)
        {
            panelChoixLsSupp.Hide();
            panelLsExProf.Hide();
            panelLogin.Hide();
            panelBody.Show();
            panelEditor.Hide();
            panelEditProf.Hide();
            panelAfficherPersonel.Hide();
            panelAddUser.Hide();
            panelEditEtudiant.Hide();
            panelMotdepassOublier.Hide();
            panelNVmdp.Hide();
            panelComande.Show();
            panelMotdepassOublier.Hide();
            panelChoixList.Hide();
            panelListEtudiant.Show();
            panelLsEtudSupp.Hide();
            dataGridView2.Rows.Clear();
            foreach (Etudiant x in EtudiantList)
            {
                if (x.ex == true)
                    dataGridView2.Rows.Add(x.ID, x.Nom, x.Prenom, x.Niveau, "0" + x.telephone, x.NomTuteur, x.Email, x.autre);
            }

        }


        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];

                string id = dataGridView2.SelectedCells[1].Value.ToString();


                foreach (Etudiant x in EtudiantList)
                {
                    if (x.Nom == id)
                    {
                        textBoxEtudNom.Text = "";
                        textBoxEtudPrenom.Text = "";
                        textBoxEtudNomPer.Text = "";
                        textBoxEtudTel.Text = "";
                        textBoxEtudMassar.Text = "";
                        textBoxEtudFrai.Text = textBoxEtudEmail.Text = "";
                        comboBox5.SelectedIndex = 0;
                        comboBox6.SelectedIndex = 0;
                        radioButton5.Checked = radioButton6.Checked = radioButton7.Checked = radioButton8.Checked = radioButton9.Checked = radioButton10.Checked = radioButton11.Checked = false;
                        textBoxEtudAge.Text = "0";
                        pictureBox4.Image = x.URL;
                        textBoxEtudAdress.Text = "";
                        buttonEtudValide.Hide();

                        buttonEtudSup.Show();
                        buttonEtudModifier.Show();
                        buttonEtudAplik.Show();
                        buttonEtudModifier.Enabled = true;
                        buttonEtudSup.Enabled = true;
                        panelLsEtudSupp.Hide();
                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        panelLogin.Hide();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Hide();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelEditEtudiant.Show();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Hide();
                        panelComande.Show();
                        panelMotdepassOublier.Hide();
                        panelListEtudiant.Hide();
                        panelChoixList.Hide();
                        dateTimePicker2.Value = DateTime.Today;


                        textBoxEtudNom.Enabled = false;
                        textBoxEtudPrenom.Enabled = false;
                        textBoxEtudNomPer.Enabled = false;
                        textBoxEtudTel.Enabled = false;
                        textBoxEtudMassar.Enabled = false;
                        textBoxEtudFrai.Enabled = false;
                        textBoxEtudEmail.Enabled = false;
                        dateTimePicker2.Enabled = false;
                        comboBox5.Enabled = false;
                        comboBox6.Enabled = false;
                        radioButton5.Enabled = false;
                        radioButton6.Enabled = false;
                        radioButton7.Enabled = false;
                        radioButton8.Enabled = false;
                        radioButton9.Enabled = false;
                        radioButton10.Enabled = false;
                        radioButton11.Enabled = false;
                        textBoxEtudAge.Enabled = false;
                        textBoxEtudAdress.Enabled = false;
                        textBoxEtudFrai.Enabled = false;




                        textBoxEtudNom.Text = x.Nom;
                        textBoxEtudPrenom.Text = x.Prenom;
                        textBoxEtudNomPer.Text = x.NomTuteur;
                        textBoxEtudTel.Text = Convert.ToString("0" + x.telephone);
                        textBoxEtudMassar.Text = x.CodeMassar;
                        textBoxEtudID.Text = x.ID;
                        // Email
                        string[] gmail = x.Email.Split('@');
                        string s = gmail[0];

                        string c = gmail[1];
                        textBoxEtudEmail.Text = s;
                        comboBox6.SelectedItem = c;
                        //======================================
                        comboBox5.SelectedIndex = x.Niveau;
                        dateTimePicker2.Value = x.dateNaissance;
                        textBoxEtudAge.Text = Convert.ToString(DateTime.Now.Year - dateTimePicker2.Value.Year);


                        if (x.Type == true)
                            radioButton5.Checked = true;
                        else
                            radioButton6.Checked = true;

                        textBoxEtudAdress.Text = x.adress;

                        if (x.Physique == true)
                            radioButton7.Checked = true;
                        else
                            radioButton8.Checked = true;

                        if (x.autre == 0) { }
                        if (x.autre == 1)
                        {
                            radioButton9.Checked = true;
                            radioButton10.Checked = false;
                            radioButton11.Checked = false;
                        }
                        if (x.autre == 2)
                        {
                            radioButton9.Checked = false;
                            radioButton10.Checked = true;
                            radioButton11.Checked = false;
                        }
                        if (x.autre == 3)
                        {
                            radioButton9.Checked = false;
                            radioButton10.Checked = false;
                            radioButton11.Checked = true;
                        }

                        textBoxEtudFrai.Text = Convert.ToString(x.frai);

                    }
                }
            }
        }

        private void buttonEtudModifier_Click(object sender, EventArgs e)
        {
            textBoxEtudNom.Enabled = true;
            textBoxEtudPrenom.Enabled = true;
            textBoxEtudNomPer.Enabled = true;
            textBoxEtudTel.Enabled = true;
            textBoxEtudMassar.Enabled = true;
            textBoxEtudEmail.Enabled = true;
            comboBox5.Enabled = true;
            comboBox6.Enabled = true;
            radioButton5.Enabled = true;
            radioButton6.Enabled = true;
            radioButton7.Enabled = true;
            radioButton8.Enabled = true;
            radioButton9.Enabled = true;
            radioButton10.Enabled = true;
            radioButton11.Enabled = true;
            textBoxEtudAge.Enabled = true;
            textBoxEtudAdress.Enabled = true;
            buttonEtudAplik.Enabled = true;
            dateTimePicker2.Enabled = true;
            buttonEtudAplik.Show();


        }

        private void buttonEtudAplik_Click(object sender, EventArgs e)
        {
            bool verife = true;
            string a;

            foreach (Etudiant x in EtudiantList)
            {
                a = x.CodeMassar;
                if (x.CodeMassar == textBoxEtudMassar.Text && x.CodeMassar != a)
                {
                    textBoxEtudMassar.BackColor = Color.Red;
                    MessageBox.Show("Code Massar existant", "Ereur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    verife = false;
                }
                if (x.telephone == int.Parse(textBoxEtudTel.Text) || textBoxEtudTel.Text == "")
                {
                    textBoxEtudTel.BackColor = Color.Red;
                    MessageBox.Show("Le numero de telephone existe déja dans la base de donné , voulez vous continuer ?", "Surcharge", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                }
            }
            if (int.Parse(textBoxEtudAge.Text) > 12 || int.Parse(textBoxEtudAge.Text) < 5)
            {
                textBoxEtudAge.BackColor = Color.Red;
                verife = false;
            }
            if (radioButton5.Checked == false && radioButton6.Checked == false)
            {
                verife = false;
            }
            if (radioButton7.Checked == false && radioButton8.Checked == false)
            {
                verife = false;
            }
            if (textBoxEtudFrai.Text == "")
            {
                textBoxEtudFrai.BackColor = Color.Red;
                verife = false;
            }
            if (comboBox6.SelectedIndex == 0)
            {
                comboBox6.BackColor = Color.Red;
                verife = false;
            }
            if (comboBox5.SelectedIndex == 0)
            {
                comboBox5.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudNom.Text == "")
            {
                textBoxEtudNom.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudPrenom.Text == "")
            {
                textBoxEtudPrenom.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudNomPer.Text == "")
            {
                textBoxEtudNomPer.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudTel.Text == "" || textBoxEtudTel.Text.Length != 10 || textBoxEtudTel.Text.Substring(0, 2) != "06")
            {
                textBoxEtudTel.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudAdress.Text == "" || textBoxEtudAdress.Text.Length < 10)
            {
                textBoxEtudAdress.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudMassar.Text == "")
            {
                textBoxEtudMassar.BackColor = Color.Red;
                verife = false;
            }
            if (textBoxEtudEmail.Text == "")
            {
                textBoxEtudEmail.BackColor = Color.Red;
                verife = false;
            }
            if (verife == true)
            {
                foreach (Etudiant x in EtudiantList)
                {
                    if (x.ID == textBoxEtudID.Text)
                    {
                        x.URL = pictureBox4.Image;
                        x.Nom = textBoxEtudNom.Text;
                        x.Prenom = textBoxEtudPrenom.Text;
                        x.NomTuteur = textBoxEtudNomPer.Text;
                        x.telephone = int.Parse(textBoxEtudTel.Text);
                        x.CodeMassar = textBoxEtudMassar.Text;
                        x.Email = textBoxEtudEmail.Text + "@" + comboBox6.SelectedItem;
                        x.Niveau = comboBox5.SelectedIndex;
                        x.dateNaissance = dateTimePicker2.Value;
                        x.Type = radioButton5.Checked;
                        x.adress = textBoxEtudAdress.Text;
                        x.URL = pictureBox4.Image;
                        x.Physique = radioButton7.Checked;
                        if (radioButton9.Checked == true)
                            x.autre = 1;
                        if (radioButton10.Checked == true)
                            x.autre = 2;
                        if (radioButton11.Checked == true)
                            x.autre = 3;
                        x.frai = int.Parse(textBoxEtudFrai.Text);
                        x.URL = pictureBox4.Image;

                        FileStream f_Etud = File.Create(@"DATA/EtudiantList.bin");
                        BinaryFormatter bf_Etud = new BinaryFormatter();
                        bf_Etud.Serialize(f_Etud, EtudiantList);
                        f_Etud.Close();
                        panelLsEtudSupp.Hide();
                        panelLogin.Hide();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Hide();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelEditEtudiant.Hide();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Hide();
                        panelComande.Show();
                        panelMotdepassOublier.Hide();
                        panelChoixList.Hide();
                        panelListEtudiant.Show();
                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        dataGridView2.Rows.Clear();
                        foreach (Etudiant y in EtudiantList)
                        {
                            dataGridView2.Rows.Add(y.ID, y.Nom, x.Prenom, y.Niveau, "0" + y.telephone, y.NomTuteur, y.Email, y.autre);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Veilliez remplire tout les champ !", "Operateur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBox5.BackColor = Color.White;
                textBoxEtudAdress.BackColor = Color.White;
                textBoxEtudNomPer.BackColor = Color.White;
                textBoxEtudFrai.BackColor = Color.White;
                comboBox6.BackColor = Color.White;
                textBoxEtudEmail.BackColor = Color.White;
                textBoxEtudAge.BackColor = Color.White;
                textBoxEtudTel.BackColor = Color.White;
                textBoxEtudMassar.BackColor = Color.White;
                textBoxEtudNom.BackColor = Color.White;
                textBoxEtudPrenom.BackColor = Color.White;
            }
            dataGridView2.Rows.Clear();
            foreach (Etudiant x in EtudiantList)
            {
                dataGridView2.Rows.Add(x.ID, x.Nom, x.Prenom, x.Niveau, x.telephone, x.NomTuteur, x.Email, x.autre);
            }
        }

        private void buttonEtudAV_Click(object sender, EventArgs e)
        {
            int a;
            string id = textBoxEtudID.Text;


            string temp = id.Split('E')[1];
            a = int.Parse(temp);

            int c = EtudiantList.Count;

            if (a == 1)
            {
                textBoxEtudID.Text = EtudiantList.Last().ID;
            }

            foreach (Etudiant x in EtudiantList)
            {
                if (x.ID == textBoxEtudID.Text)
                {

                    textBoxEtudID.Text = x.ID;
                    textBoxEtudNom.Text = x.Nom;
                    textBoxEtudPrenom.Text = x.Prenom;
                    textBoxEtudNomPer.Text = x.NomTuteur;
                    textBoxEtudTel.Text = Convert.ToString(x.telephone);
                    textBoxEtudMassar.Text = x.CodeMassar;
                    x.Email = textBoxEtudEmail.Text + "@" + comboBox6.SelectedItem;
                    comboBox5.SelectedIndex = x.Niveau;
                    dateTimePicker2.Value = x.dateNaissance;
                    if (x.Type == true)
                        radioButton5.Checked = true;
                    else
                        radioButton5.Checked = false;
                    textBoxEtudAdress.Text = x.adress;
                    radioButton7.Checked = x.Physique;

                    if (x.autre == 1)
                        radioButton9.Checked = true;
                    if (x.autre == 2)
                        radioButton10.Checked = true;
                    if (x.autre == 3)
                        radioButton11.Checked = true;
                    textBoxEtudFrai.Text = Convert.ToString(x.frai);
                }
            }



        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                string id = dataGridView1.SelectedCells[0].Value.ToString();


                foreach (Prof x in ProfList)
                {
                    if (x.ID == id)
                    {

                        panelLogin.Hide();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Show();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelEditEtudiant.Hide();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Hide();
                        panelComande.Show();
                        panelMotdepassOublier.Hide();
                        panelChoixList.Hide();
                        panelListEtudiant.Hide();
                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        panelLsEtudSupp.Hide();
                        buttonProfApli.Show();

                        dateTimePicker1.Value = DateTime.Today;
                        textBoxProfNom.Text = "";
                        textBoxProfPrenom.Text = "";
                        textBoxCIN.Text = "";
                        textBoxProfGSM.Text = "";
                        textBoxProfMail.Text = "";
                        comboBox3.SelectedIndex = 0;
                        textBoxProfAdress.Text = "";
                        comboBox1.SelectedIndex = 0;
                        textBoxSonAge.Text = "";
                        comboBox2.SelectedIndex = 0;
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                        textBoxSalMois.Text = "";

                        dateTimePicker1.Enabled = false;
                        textBoxProfNom.Enabled = false;
                        textBoxProfPrenom.Enabled = false;
                        textBoxCIN.Enabled = false;
                        textBoxProfGSM.Enabled = false;
                        textBoxProfMail.Enabled = false;
                        comboBox3.Enabled = false;
                        textBoxProfAdress.Enabled = false;
                        comboBox1.Enabled = false;
                        textBoxSonAge.Enabled = false;
                        comboBox2.Enabled = false;
                        radioButton1.Enabled = false;
                        radioButton2.Enabled = false;
                        textBoxSalMois.Enabled = false;



                        pictureBox3.Image = x.lienIMG;
                        textBoxID.Text = x.ID;
                        textBoxProfNom.Text = x.Nom;
                        textBoxProfPrenom.Text = x.Prenom;
                        textBoxCIN.Text = x.CIN;
                        textBoxProfGSM.Text = "0" + x.Tel;
                        string[] gmail = x.mail.Split('@');
                        string s = gmail[0];

                        string c = gmail[1];
                        textBoxProfMail.Text = s;
                        if (c == "Gmail.com")
                            comboBox3.SelectedIndex = 1;
                        if (c == "Yahoo.fr")
                            comboBox3.SelectedIndex = 2;
                        if (c == "Hotmail.com")
                            comboBox3.SelectedIndex = 3;
                        if (c == "taalim.ma")
                            comboBox3.SelectedIndex = 4;

                        textBoxProfAdress.Text = x.Adress;
                        dateTimePicker1.Value = x.DateNaissance;
                        if (x.Niveau == "CE1")
                            comboBox1.SelectedIndex = 1;
                        if (x.Niveau == "CE2")
                            comboBox1.SelectedIndex = 2;
                        if (x.Niveau == "CE3")
                            comboBox1.SelectedIndex = 3;
                        if (x.Niveau == "CM1")
                            comboBox1.SelectedIndex = 4;
                        if (x.Niveau == "CM2")
                            comboBox1.SelectedIndex = 5;
                        if (x.Niveau == "CM3")
                            comboBox1.SelectedIndex = 6;

                        if (x.Durrer == 2)
                            radioButton1.Checked = true;
                        else
                            radioButton2.Checked = true;
                        textBoxSalMois.Text = x.Salaire.ToString();

                        if (x.matier == "Francais")
                            comboBox2.SelectedIndex = 1;
                        if (x.matier == "Arabe")
                            comboBox2.SelectedIndex = 2;
                        if (x.matier == "Mathematique")
                            comboBox2.SelectedIndex = 3;
                    }
                }

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

            dateTimePicker1.Enabled = true;
            textBoxProfNom.Enabled = true;
            textBoxProfPrenom.Enabled = true;
            textBoxCIN.Enabled = true;
            textBoxProfGSM.Enabled = true;
            textBoxProfMail.Enabled = true;
            comboBox3.Enabled = true;
            textBoxProfAdress.Enabled = true;
            comboBox1.Enabled = true;
            textBoxSonAge.Enabled = true;
            comboBox2.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            textBoxSalMois.Enabled = true;
        }

        private void buttonProfApli_Click(object sender, EventArgs e)
        {
            bool valid = true; // on verifie si tout les champs on été remplis 
            if (textBoxProfNom.Text == "")
            {
                textBoxProfNom.BackColor = Color.Red;
                valid = false;
            }
            if (textBoxProfPrenom.Text == "")
            {
                textBoxProfPrenom.BackColor = Color.Red;
                valid = false;
            }
            foreach (Prof n in ProfList)
            {

                if (textBoxCIN.Text == "" || n.CIN == textBoxCIN.Text)
                {
                    if (MessageBox.Show("Numéro CIN déja existant , Continuer ?", "OPERATEUR", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                    {
                        textBoxCIN.BackColor = Color.Red;// on verifie si tout les champs on été remplis 
                        valid = false;
                    }
                }
            }
            foreach (Prof p in ProfList)
            {
                if (p.Tel == int.Parse(textBoxProfGSM.Text))
                {
                    if (MessageBox.Show("Numéro GSM déja existant , Continuer ?", "OPERATEUR", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                    {
                        textBoxProfGSM.BackColor = Color.Red;
                        valid = false;
                    }
                }
            }
            if (textBoxProfGSM.Text == "" || textBoxProfGSM.Text.Length != 10 || textBoxProfGSM.Text.Substring(0, 2) != "06")
            {
                textBoxProfGSM.BackColor = Color.Red;
                valid = false;

            }
            if (textBoxProfMail.Text == "" || comboBox3.SelectedIndex == 0)
            {
                textBoxProfMail.BackColor = Color.Red;
                comboBox3.BackColor = Color.Red;
                valid = false;
            }
            if (textBoxProfAdress.Text == "" || textBoxProfAdress.Text.Length < 10)// on verifie si tout les champs on été remplis 
            {
                textBoxProfAdress.BackColor = Color.Red;
                valid = false;
            }
            if (comboBox1.SelectedIndex == 0)
            {
                comboBox1.BackColor = Color.Red;
                valid = false;
            }
            int a = int.Parse(textBoxSonAge.Text);
            int b = int.Parse(textBoxSonAge.Text);// on verifie si tout les champs on été remplis 
            if (a < 21 || b > 60)
            {
                textBoxSonAge.BackColor = Color.Red;
                valid = false;
            }
            if (comboBox2.SelectedIndex == 0)
            {
                comboBox2.BackColor = Color.Red;
                valid = false;
            }
            if (textBoxDurreContrat.Text == "")
            {
                textBoxDurreContrat.BackColor = Color.Red;
                valid = false;
            }
            if (textBoxSalMois.Text == "")
            {
                textBoxSalMois.BackColor = Color.Red;
                valid = false;
            }


            if (valid == true) // tout est bien dans sa place !
            {
                foreach (Prof y in ProfList)
                {
                    if (y.ID == textBoxID.Text)
                    {
                        y.ID = textBoxID.Text;
                        y.Nom = textBoxProfNom.Text;
                        y.Prenom = textBoxProfPrenom.Text;
                        y.CIN = textBoxCIN.Text;
                        y.Tel = int.Parse(textBoxProfGSM.Text);
                        y.mail = textBoxProfMail.Text + "@" + comboBox3.SelectedItem;
                        y.Adress = textBoxProfAdress.Text;
                        y.Niveau = comboBox1.SelectedIndex.ToString();
                        y.DateNaissance = dateTimePicker1.Value;
                        y.matier = comboBox2.SelectedIndex.ToString();
                        y.Durrer = int.Parse(textBoxDurreContrat.Text);
                        y.Salaire = int.Parse(textBoxSalMois.Text);
                        y.Niveau = comboBox1.SelectedItem + "";
                        y.lienIMG = pictureBox3.Image;
                        if (comboBox1.SelectedIndex == 0)
                            MessageBox.Show("ereur");
                        else
                        {
                            y.ID = textBoxID.Text;
                            y.Nom = textBoxProfNom.Text;
                            y.Prenom = textBoxProfPrenom.Text;
                            y.CIN = textBoxCIN.Text;
                            y.Tel = int.Parse(textBoxProfGSM.Text);
                            y.mail = textBoxProfMail.Text + "@" + comboBox3.SelectedItem;
                            y.Adress = textBoxProfAdress.Text;
                            y.Niveau = comboBox1.SelectedItem.ToString();
                            y.DateNaissance = dateTimePicker1.Value;
                            y.matier = comboBox2.SelectedItem + "";
                            y.Durrer = int.Parse(textBoxDurreContrat.Text);
                            y.Salaire = int.Parse(textBoxSalMois.Text);
                            y.Niveau = comboBox1.SelectedItem + "";


                            MessageBox.Show("apliqué");

                            panelAfficherPersonel.Hide();
                            panelChoixList.Show();
                            panelEditProf.Hide();
                            panelChoixLsSupp.Hide();
                            panelLsExProf.Hide();
                        }


                    }

                }
            }
            else
            {
                MessageBox.Show("4alta chi 7aja  !!", "HAHAHA", MessageBoxButtons.OK, MessageBoxIcon.Error); // on as trouvé des champs vide !
                textBoxProfNom.BackColor = Color.White;
                textBoxProfPrenom.BackColor = Color.White;
                textBoxCIN.BackColor = Color.White;
                textBoxProfGSM.BackColor = Color.White;
                textBoxProfMail.BackColor = Color.White;
                textBoxProfAdress.BackColor = Color.White;
                comboBox1.BackColor = Color.White;
                dateTimePicker1.BackColor = Color.White;
                comboBox2.BackColor = Color.White;
                textBoxDurreContrat.BackColor = Color.White;
                textBoxSalMois.BackColor = Color.White;
                textBoxSonAge.BackColor = Color.White;
                comboBox3.BackColor = Color.White;
            }
            FileStream f_prof = File.Create(@"DATA/profList.bin");
            BinaryFormatter bf_prof = new BinaryFormatter();
            bf_prof.Serialize(f_prof, ProfList);
            f_prof.Close();

            dataGridView1.Rows.Clear();
            foreach (Prof x in ProfList)
            {
                dataGridView1.Rows.Add(x.ID, x.Nom, x.Prenom, x.Niveau, x.matier, x.Salaire, x.Durrer);
            }
        }

        private void textBoxEtudID_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonProfIMG_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Image img = Image.FromFile(ofd.FileName);
                pictureBox3.Image = img;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Image img = Image.FromFile(ofd.FileName);
                pictureBox4.Image = img;
            }
        }

        private void buttonEtudSup_Click(object sender, EventArgs e)
        {

            foreach (Etudiant x in EtudiantList)
            {
                if (x.ID == textBoxEtudID.Text)
                {
                    if (x.ex == true)
                    {
                        panelLsEtudSupp.Hide();
                        panelLogin.Hide();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Hide();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelEditEtudiant.Hide();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Hide();
                        panelComande.Show();
                        panelMotdepassOublier.Hide();
                        panelChoixList.Hide();

                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        x.ex = false;
                    }
                    else
                    {
                        panelLsEtudSupp.Hide();
                        panelLogin.Hide();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Hide();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelEditEtudiant.Hide();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Hide();
                        panelComande.Show();
                        panelMotdepassOublier.Hide();
                        panelChoixList.Hide();

                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        if (MessageBox.Show("action irreversible", "Opérateur", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            EtudiantList.Remove(x);
                            break;
                        }
                    }
                }
            }
            FileStream f_Etud = File.Create(@"DATA/EtudiantList.bin");
            BinaryFormatter bf_Etud = new BinaryFormatter();
            bf_Etud.Serialize(f_Etud, EtudiantList);
            f_Etud.Close();

        }

        private void buttonListEtudiant_MouseEnter(object sender, EventArgs e)
        {
            panelChoixList.BackColor = Color.Green;
        }

        private void buttonListEtudiant_MouseLeave(object sender, EventArgs e)
        {
            panelChoixList.BackColor = Color.Gainsboro;
        }

        private void buttonLsExP_Click(object sender, EventArgs e)
        {
            panelLogin.Hide();
            panelBody.Show();
            panelEditor.Hide();
            panelEditProf.Hide();
            panelAfficherPersonel.Hide();
            panelAddUser.Hide();
            panelAfficherPersonel.Hide();
            panelMotdepassOublier.Hide();
            panelNVmdp.Hide();
            panelComande.Show();
            panelChoixList.Hide();
            panelEditEtudiant.Hide();
            panelListEtudiant.Hide();
            panelLsEtudSupp.Hide();
            panelChoixLsSupp.Hide();
            panelLsExProf.Show();

            dataGridView3.Rows.Clear();
            foreach (Prof x in ProfList)
            {
                if (x.ex == false)
                {
                    int temp = x.Debut.Year - x.fin.Year;
                    dataGridView3.Rows.Add(x.ID, x.Nom + " " + x.Prenom, x.CIN, x.Tel, x.mail, temp, x.Salaire);
                }
            }




        }

        private void buttonLsExE_Click(object sender, EventArgs e)
        {
            panelLogin.Hide();
            panelBody.Show();
            panelEditor.Hide();
            panelEditProf.Hide();
            panelAfficherPersonel.Hide();
            panelAddUser.Hide();
            panelAfficherPersonel.Hide();
            panelMotdepassOublier.Hide();
            panelChoixList.Hide();
            panelNVmdp.Hide();
            panelComande.Show();
            panelChoixList.Hide();
            panelEditEtudiant.Hide();
            panelListEtudiant.Hide();
            panelEditEtudiant.Hide();
            panelChoixLsSupp.Hide();
            panelLsExProf.Hide();
            panelLsEtudSupp.Show();



            dataGridView4.Rows.Clear();
            foreach (Etudiant x in EtudiantList)
            {
                if (x.ex == false)
                {
                    dataGridView4.Rows.Add(x.ID, x.Nom + " " + x.Prenom, x.CodeMassar, x.NomTuteur, x.telephone, x.Email, x.autre);
                }
            }

        }

        private void buttonBackUp_Click(object sender, EventArgs e)
        {
            panelLogin.Hide();
            panelBody.Show();
            panelEditor.Hide();
            panelEditProf.Hide();
            panelAfficherPersonel.Hide();
            panelAddUser.Hide();
            panelAfficherPersonel.Hide();
            panelMotdepassOublier.Hide();
            panelChoixList.Hide();
            panelNVmdp.Hide();
            panelComande.Show();
            panelChoixList.Hide();
            panelEditEtudiant.Hide();
            panelListEtudiant.Hide();
            panelEditEtudiant.Hide();
            panelChoixLsSupp.Show();
            panelLsExProf.Hide();
            panelLsEtudSupp.Hide();
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.dataGridView3.Rows[e.RowIndex];

                string id = dataGridView3.SelectedCells[0].Value.ToString();


                foreach (Prof x in ProfList)
                {
                    if (x.ID == id)
                    {

                        panelLogin.Hide();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Show();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelEditEtudiant.Hide();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Hide();
                        panelComande.Show();
                        panelMotdepassOublier.Hide();
                        panelChoixList.Hide();
                        panelListEtudiant.Hide();
                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        panelLsEtudSupp.Hide();
                        buttonProfApli.Show();

                        dateTimePicker1.Value = DateTime.Today;
                        textBoxProfNom.Text = "";
                        textBoxProfPrenom.Text = "";
                        textBoxCIN.Text = "";
                        textBoxProfGSM.Text = "";
                        textBoxProfMail.Text = "";
                        comboBox3.SelectedIndex = 0;
                        textBoxProfAdress.Text = "";
                        comboBox1.SelectedIndex = 0;
                        textBoxSonAge.Text = "";
                        comboBox2.SelectedIndex = 0;
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                        textBoxSalMois.Text = "";

                        dateTimePicker1.Enabled = false;
                        textBoxProfNom.Enabled = false;
                        textBoxProfPrenom.Enabled = false;
                        textBoxCIN.Enabled = false;
                        textBoxProfGSM.Enabled = false;
                        textBoxProfMail.Enabled = false;
                        comboBox3.Enabled = false;
                        textBoxProfAdress.Enabled = false;
                        comboBox1.Enabled = false;
                        textBoxSonAge.Enabled = false;
                        comboBox2.Enabled = false;
                        radioButton1.Enabled = false;
                        radioButton2.Enabled = false;
                        textBoxSalMois.Enabled = false;



                        pictureBox3.Image = x.lienIMG;
                        textBoxID.Text = x.ID;
                        textBoxProfNom.Text = x.Nom;
                        textBoxProfPrenom.Text = x.Prenom;
                        textBoxCIN.Text = x.CIN;
                        textBoxProfGSM.Text = "0" + x.Tel;
                        string[] gmail = x.mail.Split('@');
                        string s = gmail[0];

                        string c = gmail[1];
                        textBoxProfMail.Text = s;
                        if (c == "Gmail.com")
                            comboBox3.SelectedIndex = 1;
                        if (c == "Yahoo.fr")
                            comboBox3.SelectedIndex = 2;
                        if (c == "Hotmail.com")
                            comboBox3.SelectedIndex = 3;
                        if (c == "taalim.ma")
                            comboBox3.SelectedIndex = 4;

                        textBoxProfAdress.Text = x.Adress;
                        dateTimePicker1.Value = x.DateNaissance;
                        if (x.Niveau == "CE1")
                            comboBox1.SelectedIndex = 1;
                        if (x.Niveau == "CE2")
                            comboBox1.SelectedIndex = 2;
                        if (x.Niveau == "CE3")
                            comboBox1.SelectedIndex = 3;
                        if (x.Niveau == "CM1")
                            comboBox1.SelectedIndex = 4;
                        if (x.Niveau == "CM2")
                            comboBox1.SelectedIndex = 5;
                        if (x.Niveau == "CM3")
                            comboBox1.SelectedIndex = 6;

                        if (x.Durrer == 2)
                            radioButton1.Checked = true;
                        else
                            radioButton2.Checked = true;
                        textBoxSalMois.Text = x.Salaire.ToString();

                        if (x.matier == "Francais")
                            comboBox2.SelectedIndex = 1;
                        if (x.matier == "Arabe")
                            comboBox2.SelectedIndex = 2;
                        if (x.matier == "Mathematique")
                            comboBox2.SelectedIndex = 3;
                    }
                }

            }
        }

        private void buttonLsExP_MouseEnter(object sender, EventArgs e)
        {
            panelChoixLsSupp.BackColor = Color.Brown;
        }

        private void buttonLsExP_MouseLeave(object sender, EventArgs e)
        {
            panelChoixLsSupp.BackColor = Color.Gainsboro;
        }

        private void buttonLsExE_MouseEnter(object sender, EventArgs e)
        {
            panelChoixLsSupp.BackColor = Color.Goldenrod;
        }

        private void buttonLsExE_MouseLeave(object sender, EventArgs e)
        {
            panelChoixLsSupp.BackColor = Color.Gainsboro;
        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.dataGridView4.Rows[e.RowIndex];

                string id = dataGridView4.SelectedCells[1].Value.ToString();


                foreach (Etudiant x in EtudiantList)
                {
                    if (x.Nom == id)
                    {
                        textBoxEtudNom.Text = "";
                        textBoxEtudPrenom.Text = "";
                        textBoxEtudNomPer.Text = "";
                        textBoxEtudTel.Text = "";
                        textBoxEtudMassar.Text = "";
                        textBoxEtudFrai.Text = textBoxEtudEmail.Text = "";
                        comboBox5.SelectedIndex = 0;
                        comboBox6.SelectedIndex = 0;
                        radioButton5.Checked = radioButton6.Checked = radioButton7.Checked = radioButton8.Checked = radioButton9.Checked = radioButton10.Checked = radioButton11.Checked = false;
                        textBoxEtudAge.Text = "0";
                        pictureBox4.Image = x.URL;
                        textBoxEtudAdress.Text = "";
                        buttonEtudValide.Hide();

                        buttonEtudSup.Show();
                        buttonEtudModifier.Show();
                        buttonEtudAplik.Show();
                        buttonEtudModifier.Enabled = true;
                        buttonEtudSup.Enabled = true;
                        panelLsEtudSupp.Hide();
                        panelChoixLsSupp.Hide();
                        panelLsExProf.Hide();
                        panelLogin.Hide();
                        panelBody.Show();
                        panelEditor.Hide();
                        panelEditProf.Hide();
                        panelAfficherPersonel.Hide();
                        panelAddUser.Hide();
                        panelEditEtudiant.Show();
                        panelMotdepassOublier.Hide();
                        panelNVmdp.Hide();
                        panelComande.Show();
                        panelMotdepassOublier.Hide();
                        panelListEtudiant.Hide();
                        panelChoixList.Hide();
                        dateTimePicker2.Value = DateTime.Today;


                        textBoxEtudNom.Enabled = false;
                        textBoxEtudPrenom.Enabled = false;
                        textBoxEtudNomPer.Enabled = false;
                        textBoxEtudTel.Enabled = false;
                        textBoxEtudMassar.Enabled = false;
                        textBoxEtudFrai.Enabled = false;
                        textBoxEtudEmail.Enabled = false;
                        dateTimePicker2.Enabled = false;
                        comboBox5.Enabled = false;
                        comboBox6.Enabled = false;
                        radioButton5.Enabled = false;
                        radioButton6.Enabled = false;
                        radioButton7.Enabled = false;
                        radioButton8.Enabled = false;
                        radioButton9.Enabled = false;
                        radioButton10.Enabled = false;
                        radioButton11.Enabled = false;
                        textBoxEtudAge.Enabled = false;
                        textBoxEtudAdress.Enabled = false;
                        textBoxEtudFrai.Enabled = false;




                        textBoxEtudNom.Text = x.Nom;
                        textBoxEtudPrenom.Text = x.Prenom;
                        textBoxEtudNomPer.Text = x.NomTuteur;
                        textBoxEtudTel.Text = Convert.ToString("0" + x.telephone);
                        textBoxEtudMassar.Text = x.CodeMassar;
                        textBoxEtudID.Text = x.ID;
                        // Email
                        string[] gmail = x.Email.Split('@');
                        string s = gmail[0];

                        string c = gmail[1];
                        textBoxEtudEmail.Text = s;
                        comboBox6.SelectedItem = c;
                        //======================================
                        comboBox5.SelectedIndex = x.Niveau;
                        dateTimePicker2.Value = x.dateNaissance;
                        textBoxEtudAge.Text = Convert.ToString(DateTime.Now.Year - dateTimePicker2.Value.Year);


                        if (x.Type == true)
                            radioButton5.Checked = true;
                        else
                            radioButton6.Checked = true;

                        textBoxEtudAdress.Text = x.adress;

                        if (x.Physique == true)
                            radioButton7.Checked = true;
                        else
                            radioButton8.Checked = true;

                        if (x.autre == 0) { }
                        if (x.autre == 1)
                        {
                            radioButton9.Checked = true;
                            radioButton10.Checked = false;
                            radioButton11.Checked = false;
                        }
                        if (x.autre == 2)
                        {
                            radioButton9.Checked = false;
                            radioButton10.Checked = true;
                            radioButton11.Checked = false;
                        }
                        if (x.autre == 3)
                        {
                            radioButton9.Checked = false;
                            radioButton10.Checked = false;
                            radioButton11.Checked = true;
                        }

                        textBoxEtudFrai.Text = Convert.ToString(x.frai);

                    }
                }
            }
        }

       private void ajouterLigneEtudiant(Etudiant e)
       {
            dataGridView4.Rows.Add(e.ID, e.Nom + " " + e.Prenom, e.CodeMassar, e.NomTuteur, e.telephone, e.Email, e.autre);
       }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();

            Etudiant Recherh = new Etudiant();

            if (textBox9.Text=="")
            {
                foreach (Etudiant et in EtudiantList)
                {
                    if(et.ex == false)
                        ajouterLigneEtudiant(et);
                }
            }
            else
            {
                Recherh.Nom = textBox9.Text.Trim();
                Recherh.Prenom = textBox9.Text.Trim();
                Recherh.NomTuteur = textBox9.Text.Trim();
                Recherh.CodeMassar = textBox9.Text.Trim();
               // Recherh.telephone = int.Parse(textBox9.Text.Trim());
                foreach (Etudiant et in EtudiantList)
                {


                    if (et.Nom.Contains(Recherh.Nom) && et.ex == false)
                    {
                        ajouterLigneEtudiant(et);
                        
                    }
                    else
                    {

                        if (et.Prenom.Contains(Recherh.Prenom) && et.ex == false)
                        {
                            ajouterLigneEtudiant(et);
                            
                        }
                        else
                        {

                            if (et.NomTuteur.Contains(Recherh.NomTuteur) && et.ex == false)
                            {
                                ajouterLigneEtudiant(et);
                            }
                            else
                            {

                                if (et.CodeMassar.Contains(Recherh.CodeMassar) && et.ex == false)
                                {
                                    ajouterLigneEtudiant(et);
                                }
                                else
                                {

                                    //if (et.telephone.ToString().Contains(Recherh.telephone.ToString()))
                                    //{
                                    //    ajouterLigneEtudiant(et);
                                    //}
                                }
                            }
                        }
                    }
                }

            }












            //    foreach (Etudiant x in EtudiantList)
            //    {

            //        if (x.ex == false)
            //        {

            //            if (textBox9.Text == "")
            //            {
            //                dataGridView4.Rows.Clear();
            //                foreach (Etudiant z in EtudiantList)
            //                {

            //                    dataGridView4.Rows.Add(z.ID, z.Nom + " " + z.Prenom, z.CodeMassar, z.NomTuteur, z.telephone, z.Email, z.autre);
            //                }
            //            }
            //            else
            //            {
            //                foreach (Etudiant y in EtudiantList)
            //                {
            //                    if (recherch == y.Nom.Substring(0, textBox9.Text.Length) || recherch == y.Prenom.Substring(0, textBox9.Text.Length) || recherch.Equals(y.telephone.ToString().Substring(0, textBox9.Text.Length)) || recherch == y.ID.Substring(0, textBox9.Text.Length))
            //                    {

            //                        dataGridView4.Rows.Add(y.ID, y.Nom + " " + y.Prenom, y.CodeMassar, y.NomTuteur, y.telephone, y.Email, y.autre);

            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
















        }

     
         private void clearTextbox()
         {
            textBox13.Text = "";
         }
        private void ajouterLigneEtudiant2(Etudiant e)
        {
            dataGridView2.Rows.Add(e.ID, e.Nom , e.Prenom, e.Niveau, e.telephone, e.NomTuteur, e.Email, e.autre);
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();

            Etudiant Recherh = new Etudiant();

            
            if (textBox7.Text == "" && comboBox7.SelectedIndex == 0)
            {
                foreach (Etudiant et in EtudiantList)
                {
                    if (et.ex == true)
                        ajouterLigneEtudiant2(et);
                }
            }
            else
            {
                
                Recherh.Nom = textBox7.Text.Trim();
                Recherh.Prenom = textBox7.Text.Trim();
                Recherh.NomTuteur = textBox7.Text.Trim();
                Recherh.CodeMassar = textBox7.Text.Trim();
                // Recherh.telephone = int.Parse(textBox9.Text.Trim());
                foreach (Etudiant et in EtudiantList)
                {


                    if (et.Nom.Contains(Recherh.Nom) && et.ex == true )
                    {
                        ajouterLigneEtudiant2(et);

                    }
                    else
                    {

                        if (et.Prenom.Contains(Recherh.Prenom) && et.ex == true )
                        {
                            ajouterLigneEtudiant2(et);

                        }
                        else
                        {

                            if (et.NomTuteur.Contains(Recherh.NomTuteur) && et.ex == true )
                            {
                                ajouterLigneEtudiant2(et);
                            }
                            else
                            {

                                if (et.CodeMassar.Contains(Recherh.CodeMassar) && et.ex == true )
                                {
                                    ajouterLigneEtudiant2(et);
                                }
                                else
                                {

                                    //if (et.telephone.ToString().Contains(Recherh.telephone.ToString()))
                                    //{
                                    //    ajouterLigneEtudiant2(et);
                                    //}
                                }
                            }
                        }
                    }
                }

            }


        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Etudiant et in EtudiantList)
            {
                if(comboBox7.SelectedIndex == 0 && textBox7.Text == "")
                    ajouterLigneEtudiant2(et);
                if (et.Niveau == comboBox7.SelectedIndex && et.ex == true && textBox7.Text == "")
                    ajouterLigneEtudiant2(et);
                else
                    dataGridView2.Rows.Clear();
            }
        }














        private void ajouterLigneprof( Prof e)
        {
            dataGridView3.Rows.Add(e.ID, e.Nom+" " +e.Prenom, e.CIN, e.Tel, e.fin - e.Debut, e.Salaire);
        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();

            Prof Recherh = new Prof();


            if (textBox8.Text == "")
            {
                foreach (Prof et in ProfList)
                {
                    if (et.ex == false)
                        ajouterLigneprof(et);
                }
            }
            else
            {

                Recherh.Nom = textBox8.Text.Trim();
                Recherh.Prenom = textBox8.Text.Trim();
                Recherh.CIN = textBox8.Text.Trim();
                //Recherh.CodeMassar = textBox8.Text.Trim();
                // Recherh.telephone = int.Parse(textBox9.Text.Trim());
                foreach (Prof et in ProfList)
                {


                    if (et.Nom.Contains(Recherh.Nom) && et.ex == false)
                    {
                        ajouterLigneprof(et);

                    }
                    else
                    {

                        if (et.Prenom.Contains(Recherh.Prenom) && et.ex == false)
                        {
                            ajouterLigneprof(et);

                        }
                        else
                        {

                            if (et.CIN.Contains(Recherh.CIN) && et.ex == false)
                            {
                                ajouterLigneprof(et);
                            }
                            else
                            {

                                //if (et.CodeMassar.Contains(Recherh.CodeMassar) && et.ex == false)
                                //{
                                //    ajouterLigneEtudiant2(et);
                                //}
                                //else
                                //{

                                //    //if (et.telephone.ToString().Contains(Recherh.telephone.ToString()))
                                //    //{
                                //    //    ajouterLigneEtudiant2(et);
                                //    //}
                                //}
                            }
                        }
                    }
                }

            }
        }
















        private void ajouterLigneprof2(Prof e)
        {
            dataGridView1.Rows.Add(e.ID, e.Nom , e.Prenom,e.Niveau, e.matier, e.Salaire,e.Durrer);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)         // prof list true
        {
            dataGridView1.Rows.Clear();

            Prof Recherh = new Prof();


            if (textBox1.Text == "")
            {
                foreach (Prof et in ProfList)
                {
                    if (et.ex == true)
                        ajouterLigneprof2(et);
                }
            }
            else
            {

                Recherh.Nom = textBox1.Text.Trim();
                Recherh.Prenom = textBox1.Text.Trim();
                Recherh.CIN = textBox1.Text.Trim();
                //Recherh.CodeMassar = textBox8.Text.Trim();
                // Recherh.telephone = int.Parse(textBox9.Text.Trim());
                foreach (Prof et in ProfList)
                {


                    if (et.Nom.Contains(Recherh.Nom) && et.ex == true)
                    {
                        ajouterLigneprof2(et);

                    }
                    else
                    {

                        if (et.Prenom.Contains(Recherh.Prenom) && et.ex == true)
                        {
                            ajouterLigneprof2(et);

                        }
                        else
                        {

                            if (et.CIN.Contains(Recherh.CIN) && et.ex == true)
                            {
                                ajouterLigneprof2(et);
                            }
                            else
                            {

                                //if (et.CodeMassar.Contains(Recherh.CodeMassar) && et.ex == false)
                                //{
                                //    ajouterLigneEtudiant2(et);
                                //}
                                //else
                                //{

                                //    //if (et.telephone.ToString().Contains(Recherh.telephone.ToString()))
                                //    //{
                                //    //    ajouterLigneEtudiant2(et);
                                //    //}
                                //}
                            }
                        }
                    }
                }

            }
        }
    }

}