import { useState, useEffect } from "react"
import FilterControls from "./filterControl"
import GamesList from "./gamesList"
import "./gameCatalog.css"
import Pagination from "./pagination"

interface Game {
  id: number
  name: string
  image: string
  link: string
  price: number
  discount: number
}

interface DataGames {
  totalGames : number
  pageSize : number
  pageNumber : number
  totalPages : number
}

interface Filters {
  priceMin: number
  priceMax: number
  discount: number
}

export default function GameCatalog() {
  const [games, setGames] = useState<Game[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [page, setPage] = useState(1);
  const pageSize = 15
  const [dataGames, setDataGames] = useState<DataGames>({
    totalGames: 0,
    pageSize: 0,
    pageNumber: 0,
    totalPages: 0,
  });
  const [filters, setFilters] = useState<Filters>({
    priceMin: 0,
    priceMax: 2000000,
    discount: 0,
  })
  const [sortBy, setSortBy] = useState<"price-asc"|"price-desc"|"discount-asc"|"discount-desc"|"name-asc"|"name-desc">("name-asc")
  const [shouldFetch, setShouldFetch] = useState(true)

  // Obtener juegos de la API con filtros
  useEffect(() => {
    const fetchGames = async () => {
      if (!shouldFetch) return;
      
      try {
        setLoading(true)
        
        // Construir URL con todos los parámetros
        const params = new URLSearchParams({
          rangoMin: filters.priceMin.toString(),
          rangoMax: filters.priceMax.toString(),
          discount: filters.discount.toString(),
          pageNumber: page.toString(),
          pageSize: pageSize.toString(),
          sortBy: sortBy
        });
        
        const response = await fetch(`https://localhost:7166/api/game/filter?rangoMin=${filters.priceMin}&rangoMax=${filters.priceMax}&discount=${filters.discount}&sortBy=${sortBy}&pageNumber=${page}&pageSize=10
`)

        if (!response.ok) {
          throw new Error(`Error HTTP: ${response.status}`)
        }
        
        const data = await response.json()
        const gameData = data.items
        
        setDataGames({
          totalGames: data.totalItems,
          pageSize: data.pageSize,
          pageNumber: data.pageNumber,
          totalPages: data.totalPages,
        });

        setGames(gameData)
        setError(null)
        setShouldFetch(false)
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Error desconocido')
        console.error('Error fetching games:', err)
      } finally {
        setLoading(false)
      }
    }

    fetchGames()
  }, [page, shouldFetch])

  const handleFiltersChange = (newFilters: Filters) => {
    setFilters(newFilters)
  }

  const handleSortChange = (newSortBy: typeof sortBy) => {
    setSortBy(newSortBy)
    setShouldFetch(true)
  }

  const handleApplyFilters = () => {
    setShouldFetch(true)
  }

  if (error) {
    return (
      <div className="catalog-container">
        <div className="error-state">
          <div className="error-icon">⚠️</div>
          <h3>Error al cargar juegos</h3>
          <p>{error}</p>
          <button onClick={() => window.location.reload()}>
            Reintentar
          </button>
        </div>
      </div>
    )
  }

  return (
    <div className="catalog-container">
      <div className="catalog-header">
        <h1>Catálogo de Juegos</h1>
      </div>

      <div className="catalog-content">
        <aside className="filters-sidebar">
          <FilterControls
            filters={filters}
            sortBy={sortBy}
            onFiltersChange={handleFiltersChange}
            onSortChange={handleSortChange}
            onApplyFilters={handleApplyFilters}
            onPageReset={setPage}
          />
        </aside>

        <main className="games-main">
          <Pagination 
            totalItems={dataGames.totalGames} 
            itemsPerPage={pageSize} 
            onPageChange={setPage}  
            actualPage={page}
          />
          <GamesList games={games} loading={loading} dataGames={dataGames} />
        </main>
      </div>
    </div>
  )
}