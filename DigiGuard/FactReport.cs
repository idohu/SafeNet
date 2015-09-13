
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace DigiGuard
{

using System;
    using System.Collections.Generic;
    
public partial class FactReport
{

    public FactReport()
    {

        this.Changes = new HashSet<Change>();

    }


    public int ReportID { get; set; }

    public System.DateTime TimeStamp { get; set; }

    public string URL { get; set; }

    public string ScreenShot { get; set; }

    public Nullable<int> CategoryID { get; set; }

    public string Location { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string Phone { get; set; }

    public string Description { get; set; }

    public string Email { get; set; }

    public Nullable<int> StatusID { get; set; }



    public virtual DimCategory DimCategory { get; set; }

    public virtual DimStatu DimStatu { get; set; }

    public virtual ICollection<Change> Changes { get; set; }

}

}
