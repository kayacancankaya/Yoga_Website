using Microsoft.EntityFrameworkCore;

namespace Pan.Models.DbClasses.ViewModels
{
    public class LocationCardsViewModel
    {
        public List<Locations> Studios { get; set; }
        public List<Locations> EventPlaces { get; set; }
        private readonly PanDbContext _context;

        public LocationCardsViewModel(PanDbContext context)
        {
            _context = context;
            Studios = new ();
            EventPlaces = new ();
            foreach (Locations location in _context.Locations.Where(l=>l.LocationTypeID ==1).ToList()) 
                Studios.Add(location);

            foreach (Locations location in _context.Locations.Where(l=>l.LocationTypeID ==2).ToList()) 
                EventPlaces.Add(location);
            
        }
    }

}
