using Foci_RCS_Projekt.Data;
using Foci_RCS_Projekt.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foci_RCS_Projekt.Pages.TabellaPages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<TabellaSor> Tabella { get; set; } = new();

    public async Task OnGetAsync()
    {
        var csapatok = await _context.Csapatok
            .AsNoTracking()
            .OrderBy(c => c.Nev)
            .ToListAsync();

        var merkozesek = await _context.Merkozesek
            .AsNoTracking()
            .Where(m => m.HazaiGol.HasValue &&
                        m.VendegGol.HasValue)
            .ToListAsync();

        foreach (var csapat in csapatok)
        {
            var sajatMeccsek = merkozesek
                .Where(m => m.HazaiCsapatId == csapat.Id ||
                            m.VendegCsapatId == csapat.Id)
                .ToList();

            int l = sajatMeccsek.Count;
            int gy = 0;
            int d = 0;
            int v = 0;
            int rg = 0;
            int kg = 0;

            foreach (var meccs in sajatMeccsek)
            {
                bool hazai = meccs.HazaiCsapatId == csapat.Id;

                int sajatGol = hazai
                    ? meccs.HazaiGol!.Value
                    : meccs.VendegGol!.Value;

                int ellenfelGol = hazai
                    ? meccs.VendegGol!.Value
                    : meccs.HazaiGol!.Value;

                rg += sajatGol;
                kg += ellenfelGol;

                if (sajatGol > ellenfelGol)
                    gy++;
                else if (sajatGol == ellenfelGol)
                    d++;
                else
                    v++;
            }

            Tabella.Add(new TabellaSor
            {
                CsapatNev = csapat.Nev,
                Lejatszott = l,
                Gyozelem = gy,
                Dontetlen = d,
                Vereseg = v,
                RugottGol = rg,
                KapottGol = kg,
                GolKulonbseg = rg - kg,
                Pont = gy * 3 + d
            });
        }

        Tabella = Tabella
            .OrderByDescending(t => t.Pont)
            .ThenByDescending(t => t.GolKulonbseg)
            .ThenByDescending(t => t.RugottGol)
            .ThenBy(t => t.CsapatNev)
            .ToList();
    }

    public class TabellaSor
    {
        public string CsapatNev { get; set; } = string.Empty;
        public int Lejatszott { get; set; }
        public int Gyozelem { get; set; }
        public int Dontetlen { get; set; }
        public int Vereseg { get; set; }
        public int RugottGol { get; set; }
        public int KapottGol { get; set; }
        public int GolKulonbseg { get; set; }
        public int Pont { get; set; }
    }
}
