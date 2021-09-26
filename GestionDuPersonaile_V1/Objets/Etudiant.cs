using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GestionDuPersonaile_V1.Objets
{
    [Serializable]
    public class Etudiant
    {
        public string ID;
        public string Nom;
        public string Prenom;
        public string NomTuteur;
        public int telephone;
        public string CodeMassar;
        public string Email;
        public int Niveau;
        public DateTime dateNaissance;
        public bool Type;
        public string adress;
        public bool Physique;
        public int autre;
        public string FichierMedical;
        public int frai;
        public Image URL;
        public bool ex=true;

        public Etudiant() 
        {
            this.ID = "";
            this.Nom = "";
            this.Prenom = "";
            this.NomTuteur = "";
            this.telephone = 06;
            this.CodeMassar = "";
            this.Email = "";
            this.Niveau = 1;
            this.dateNaissance = DateTime.Today;
            this.Type = true;
            this.adress = "";
            this.Physique = true;
            this.autre = 0;
            this.frai = 0;
            
            this.ex = true;
        }


        public Etudiant(string ID, string Nom, string Prenom, string NomTuteur, int telephone, string CodeMassar, string Email, int Niveau, DateTime dateNaissance, bool Type, string adress, bool Physique, int autre, int frai, Image URL, bool ex)
        {
            this.ID = ID;
            this.Nom = Nom;
            this.Prenom = Prenom;
            this.NomTuteur = NomTuteur;
            this.telephone = telephone;
            this.CodeMassar=CodeMassar;
            this.Email = Email;
            this.Niveau = Niveau;
            this.dateNaissance = dateNaissance;
            this.Type = Type;
            this.adress = adress;
            this.Physique = Physique;
            this.autre = autre;
            this.frai=frai;
            this.URL = URL;
            this.ex = ex;
        }



    }
}
