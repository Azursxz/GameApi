import FilterControls from "./filterControl";
import Card from "./card";
import { useState } from "react";
import GameCatalog from "./gameCatalog";

export default function  App () {
  // Estado inicial de filtros
  const [filters, setFilters] = useState({
    priceMin: 0,
    priceMax: 20000,
    discountMin: 0,
    discountMax: 100,
  });

  // Estado de ordenamiento
  const [sortBy, setSortBy] = useState<
    "price-asc" | "price-desc" | "discount-asc" | "discount-desc" | "name-asc" | "name-desc"
  >("name-asc");

  return (
    <>

      <FilterControls
        filters={filters}
        sortBy={sortBy}
        onFiltersChange={(newFilters) => setFilters(newFilters)}
        onSortChange={(newSort) => setSortBy(newSort)}
      />

       <Card
        key={12}
        id={12}
        name={"Crowd City"}
        image={"https://store-images.s-microsoft.com/image/apps.28239.13922210868210363.c2ccdf5d-6051-417f-a330-8cbe27f045c1.c39a204c-6041-4bae-a9da-660a770891f0?q=90&w=177&h=177"}
        link={"http://www.xbox.com/es-AR/games/store/crowd-city/9N0NFX4ZPFDS/0010"}
        originalPrice={5499.00}
        discount={20}
      />

      <GameCatalog/>

    </>
  );
};