"use client"

import { useState, useEffect } from "react"
import FilterControls from "./filterControl"
import GamesList from "./gamesList"
import "./gameCatalog.css"

interface Game {
  id: number
  name: string
  image: string
  link: string
  price: number
  discount: number
}

interface Filters {
  priceMin: number
  priceMax: number
  discountMin: number
  discountMax: number
}

export default function GameCatalog() {
  const [games, setGames] = useState<Game[]>([])
  const [filteredGames, setFilteredGames] = useState<Game[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [filters, setFilters] = useState<Filters>({
    priceMin: 0,
    priceMax: 20000,
    discountMin: 0,
    discountMax: 100
  })
  const [sortBy, setSortBy] = useState<"price-asc" | "price-desc" | "discount-asc" | "discount-desc" | "name-asc" | "name-desc">("name-asc")

  // Obtener juegos de la API
  useEffect(() => {
    const fetchGames = async () => {
      try {
        setLoading(true)
        // Reemplaza con tu endpoint real
        const response = await fetch('https://localhost:7166/api/game/allgamespaginated?pageNumber=1&pageSize=25')
        
        if (!response.ok) {
          throw new Error(`Error HTTP: ${response.status}`)
        }
        
        const data = await response.json()
        const gameData = data.items

        setGames(gameData)
        setError(null)
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Error desconocido')
        console.error('Error fetching games:', err)
      } finally {
        setLoading(false)
      }
    }

    fetchGames()
  }, [])

  // Aplicar filtros y ordenamiento
  useEffect(() => {
    if (games.length === 0) return

    let result = [...games]

    // Aplicar filtros de precio
    result = result.filter(game => 
      game.price >= filters.priceMin && 
      game.price <= filters.priceMax
    )

    // Aplicar filtros de descuento
    result = result.filter(game => 
      game.discount >= filters.discountMin && 
      game.discount <= filters.discountMax
    )

    // Aplicar ordenamiento
    switch (sortBy) {
      case "price-asc":
        result.sort((a, b) => a.price - b.price)
        break
      case "price-desc":
        result.sort((a, b) => b.price - a.price)
        break
      case "discount-asc":
        result.sort((a, b) => a.discount - b.discount)
        break
      case "discount-desc":
        result.sort((a, b) => b.discount - a.discount)
        break
      case "name-desc":
        result.sort((a, b) => b.name.localeCompare(a.name))
        break
      case "name-asc":
      default:
        result.sort((a, b) => a.name.localeCompare(b.name))
        break
    }

    setFilteredGames(result)
  }, [games, filters, sortBy])

  const handleFiltersChange = (newFilters: Filters) => {
    setFilters(newFilters)
  }

  const handleSortChange = (newSortBy: typeof sortBy) => {
    setSortBy(newSortBy)
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
        <p>Encuentra los mejores juegos con descuentos increíbles</p>
      </div>

      <div className="catalog-content">
        <aside className="filters-sidebar">
          <FilterControls
            filters={filters}
            sortBy={sortBy}
            onFiltersChange={handleFiltersChange}
            onSortChange={handleSortChange}
          />
        </aside>

        <main className="games-main">
          <GamesList games={filteredGames} loading={loading} />
        </main>
      </div>
    </div>
  )
}