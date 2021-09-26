using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GestionDuPersonaile_V1.Objets
{
    [Serializable]
    public class Prof
    {
        public string ID;
        public string Nom;
        public string Prenom;
        public string CIN;
        public int Tel;
        public string mail;
        public string Adress;
        public string Niveau;
        public DateTime DateNaissance;
        public DateTime Debut;
        public DateTime fin;
        public string matier;
        public int Durrer;
        public int Salaire;
        public Image lienIMG;
        public bool ex;

        public Prof() { }

        public Prof(string ID, string Nom, string Prenom, string CIN,int Tel, string mail, string Adress, string Niveau, DateTime DateNaissance, string matier, int Durrer, int Salaire, Image img, bool ex,DateTime debut,DateTime fin)
        {
            this.ID = ID;
            this.Nom = Nom;
            this.Prenom = Prenom;
            this.CIN = CIN;
            this.Tel = Tel;
            this.mail=mail;
            this.Adress = Adress;
            this.Niveau = Niveau;
            this.DateNaissance = DateNaissance;
            this.matier = matier;
            this.Durrer = Durrer;
            this.Salaire = Salaire;
            this.lienIMG = img;
            this.ex = ex;
            this.Debut = debut;
            this.fin = fin;
        }


    }
}
