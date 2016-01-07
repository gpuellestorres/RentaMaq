namespace RentaMaq.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Primera : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.equipos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        tipoEquipo = c.String(),
                        patenteEquipo = c.String(),
                        año = c.Int(nullable: false),
                        numeroAFI = c.String(),
                        ModeloID_ModeloID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Modelo", t => t.ModeloID_ModeloID, cascadeDelete: true)
                .Index(t => t.ModeloID_ModeloID);
            
            CreateTable(
                "dbo.Modelo",
                c => new
                    {
                        ModeloID = c.String(nullable: false, maxLength: 128),
                        nombreModelo = c.String(),
                        MarcaID_MarcaID = c.Int(),
                    })
                .PrimaryKey(t => t.ModeloID)
                .ForeignKey("dbo.Marca", t => t.MarcaID_MarcaID)
                .Index(t => t.MarcaID_MarcaID);
            
            CreateTable(
                "dbo.Marca",
                c => new
                    {
                        MarcaID = c.Int(nullable: false, identity: true),
                        NombreMarca = c.String(),
                    })
                .PrimaryKey(t => t.MarcaID);
            
            CreateTable(
                "dbo.Maestro",
                c => new
                    {
                        MaestroID = c.String(nullable: false, maxLength: 128),
                        fecha = c.DateTime(nullable: false),
                        ProductoID = c.String(),
                        descripcionProducto = c.String(),
                        cantidadEntrante = c.Int(nullable: false),
                        cantidadSaliente = c.Int(nullable: false),
                        facturaDespacho = c.String(),
                        proveedor = c.String(),
                        valorUnitario = c.Int(nullable: false),
                        valorTotal = c.Int(nullable: false),
                        afiEquipo = c.String(),
                        entragadoA = c.String(),
                        motivo = c.String(),
                        numeroFormulario = c.String(),
                    })
                .PrimaryKey(t => t.MaestroID);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        ProductoID = c.String(nullable: false, maxLength: 128),
                        precioUnitario = c.Int(nullable: false),
                        descripcion = c.String(),
                        descripcion2 = c.String(),
                        stockActual = c.Int(nullable: false),
                        stockMinimo = c.Int(nullable: false),
                        unidadDeMedida = c.String(),
                        ubicacion = c.String(),
                        codigo = c.String(),
                    })
                .PrimaryKey(t => t.ProductoID);
            
            CreateTable(
                "dbo.Proveedor",
                c => new
                    {
                        ProveedorID = c.Int(nullable: false, identity: true),
                        nombreProveedor = c.String(),
                        requerimiento = c.String(),
                        personaContacto1 = c.String(),
                        personaContacto2 = c.String(),
                        telefonos = c.String(),
                        correo = c.String(),
                    })
                .PrimaryKey(t => t.ProveedorID);
            
            CreateTable(
                "dbo.reportCombustible",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        numeroReport = c.Int(nullable: false),
                        usuario = c.String(nullable: false),
                        denominacionEquipo = c.String(nullable: false),
                        horometro = c.Int(nullable: false),
                        kilometraje = c.Int(nullable: false),
                        fechaHora = c.DateTime(nullable: false),
                        litros = c.Int(nullable: false),
                        ubicacion = c.String(nullable: false),
                        operador = c.String(nullable: false),
                        quienCarga = c.String(nullable: false),
                        responsable = c.String(nullable: false),
                        comentario = c.String(nullable: false),
                        equiposID_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.equipos", t => t.equiposID_ID, cascadeDelete: true)
                .Index(t => t.equiposID_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.reportCombustible", "equiposID_ID", "dbo.equipos");
            DropForeignKey("dbo.equipos", "ModeloID_ModeloID", "dbo.Modelo");
            DropForeignKey("dbo.Modelo", "MarcaID_MarcaID", "dbo.Marca");
            DropIndex("dbo.reportCombustible", new[] { "equiposID_ID" });
            DropIndex("dbo.Modelo", new[] { "MarcaID_MarcaID" });
            DropIndex("dbo.equipos", new[] { "ModeloID_ModeloID" });
            DropTable("dbo.reportCombustible");
            DropTable("dbo.Proveedor");
            DropTable("dbo.Producto");
            DropTable("dbo.Maestro");
            DropTable("dbo.Marca");
            DropTable("dbo.Modelo");
            DropTable("dbo.equipos");
        }
    }
}
