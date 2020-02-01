using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using System.Diagnostics;
using PetGrooming.Models.ViewModels;

namespace PetGrooming.Controllers
{
    public class PetController : Controller
    {
     
        private PetGroomingContext db = new PetGroomingContext();

        // GET: Pet
        public ActionResult List()
        {
          
            List<Pet> pets = db.Pets.SqlQuery("Select * from Pets").ToList();
            return View(pets);
           
        }

       // Show details for individual pet
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            Pet pet = db.Pets.SqlQuery("select * from pets where petid=@PetID", new SqlParameter("@PetID",id)).FirstOrDefault();
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

       
        [HttpPost]
        public ActionResult Add(string PetName, Double PetWeight, String PetColor, int SpeciesID, string PetNotes)
        {
            //Query to insert values into the table
            string query = "insert into pets (PetName, Weight, color, SpeciesID, Notes) values (@PetName,@PetWeight,@PetColor,@SpeciesID,@PetNotes)";
            SqlParameter[] sqlparams = new SqlParameter[5]; 
            // key and value pairs
            sqlparams[0] = new SqlParameter("@PetName",PetName);
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
            sqlparams[3] = new SqlParameter("@SpeciesID", SpeciesID);
            sqlparams[4] = new SqlParameter("@PetNotes",PetNotes);

            //executoing the sql query with the values
            db.Database.ExecuteSqlCommand(query, sqlparams);

            
            // the below code will help you to redirect to lsit view of the page to see the newly addedd value along woth other values
            return RedirectToAction("List");
        }


        public ActionResult Add()
        {
            
            List<Species> species = db.Species.SqlQuery("select * from Species").ToList();

            return View(species);
        }

        //Update
        public ActionResult Update(int id)
        {
            //need information about a particular pet
            Pet selectedpet = db.Pets.SqlQuery("select * from pets where petid = @id", new SqlParameter("@id",id)).FirstOrDefault();
            string query="select * from species";
            List<Species> selectedspecies = db.Species.SqlQuery(query).ToList();
            UpdatePet viewmodel = new UpdatePet();
            viewmodel.pet = selectedpet;
            viewmodel.species = selectedspecies;
            return View(viewmodel);
        }
        //[HttpPost] Update
        [HttpPost]
        public ActionResult Update(int id,string PetName, string PetColor, double PetWeight,string PetNotes)
        {   //query to update pets
            string query = "update pets set PetName=@PetName, Weight=@PetWeight,Color=@PetColor,Notes=@PetNotes where PetID=@id";
            //key pair values to hold new values 
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@PetName",PetName);
            parameters[1] = new SqlParameter("@PetWeight",PetWeight);
            parameters[2] = new SqlParameter("@PetColor",PetColor);
            parameters[3] =new SqlParameter("@PetNotes",PetNotes);
            parameters[4] =new SqlParameter("@id",id);
            //Exceuting the sql query with new values
            db.Database.ExecuteSqlCommand(query,parameters);

            //Return to list view of pets
            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // (optional) delete
        public ActionResult Delete(int id)
        {   //Query to delete particualr species from the table based on the species id
            string query = "delete from pets where PetID=@id";
            SqlParameter[] parameter = new SqlParameter[1];
            //storing the id of the species to be deleted 
            parameter[0] = new SqlParameter("@id", id);
            //Excecuting the query
            db.Database.ExecuteSqlCommand(query, parameter);
            // returning to lsit view of the species after deleting 
            return RedirectToAction("List");
        }
    }
}
