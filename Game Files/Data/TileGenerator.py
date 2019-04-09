
def main():
    tile_id_base = input("Enter the base tile_id: ")
    cell_description = input("Enter the cell description: ")
    primary_name = input("Enter the primary tile name: ")
    secondary_name = input("Enter the secondary tile name: ")
    primary_tile_description = input("Enter the primary tile description")

    output_string = f"""\
            private const string {tile_id_base}_desc = 
    @"{cell_description}";
             
                // Nearton
                #region
                new Tile("{primary_name}", "{tile_id_base}_tile", string.Concat({tile_id_base}_desc, "\\n",
    @"{primary_tile_description}"),
                    town_list: new List<string>() {{ "town_{tile_id_base}" }},
                    north: "{tile_id_base}_n",
                    south: "{tile_id_base}_s",
                    east: "{tile_id_base}_e",
                    west: "{tile_id_base}_w"),
    
                new Tile("{secondary_name}", "{tile_id_base}_sw", {tile_id_base}_desc,
                    north: "{tile_id_base}_w",
                    east: "{tile_id_base}_s"),
    
                new Tile("{secondary_name}", "{tile_id_base}_s", {tile_id_base}_desc,
                    north: "{tile_id_base}_tile",
                    east: "{tile_id_base}_se",
                    west: "{tile_id_base}_sw"),
    
                new Tile("{secondary_name}", "{tile_id_base}_se", {tile_id_base}_desc,
                    north: "{tile_id_base}_e",
                    west: "{tile_id_base}_s"),
    
                new Tile("{secondary_name}", "{tile_id_base}_w", {tile_id_base}_desc,
                    north: "{tile_id_base}_nw",
                    south: "{tile_id_base}_sw",
                    east: "{tile_id_base}_tile"),
    
                new Tile("{secondary_name}", "{tile_id_base}_e", {tile_id_base}_desc,
                    north: "{tile_id_base}_ne",
                    south: "{tile_id_base}_se",
                    west: "{tile_id_base}_tile"),
    
                new Tile("{secondary_name}", "{tile_id_base}_nw", {tile_id_base}_desc,
                    south: "{tile_id_base}_w",
                    east: "{tile_id_base}_n"),
    
                new Tile("{secondary_name}", "{tile_id_base}_n", {tile_id_base}_desc,
                    south: "{tile_id_base}_tile",
                    east: "{tile_id_base}_ne",
                    west: "{tile_id_base}_nw"),
    
                new Tile("{secondary_name}", "{tile_id_base}_ne", {tile_id_base}_desc,
                    south: "{tile_id_base}_e",
                    west: "{tile_id_base}_n"),
                #endregion
                
                TileList = new List<string>() 
                {{ 
                    "{tile_id_base}_tile", 
                    "{tile_id_base}_w", 
                    "{tile_id_base}_ne", 
                    "{tile_id_base}_e", 
                    "{tile_id_base}_s", 
                    "{tile_id_base}_n", 
                    "{tile_id_base}_se", 
                    "{tile_id_base}_nw", 
                    "{tile_id_base}_sw" 
                }};"""

    f = open("output.txt", "w")
    f.write(output_string)
    f.close()


main()
