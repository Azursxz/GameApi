"use client"

import Card from "./card"
import "./gamesList.css"


interface Game {
  id: number
  name: string
  price: number
  discount: number
  link: string
  image: string
}

interface GameListProps {
  games: Game[]
  loading?: boolean
}

export default function GamesList({ games, loading = false }: GameListProps) {
  if (loading) {
    return (
      <div className="product-list-container">
        <div className="loading-state">
          <div className="loading-spinner"></div>
          <p>Cargando productos...</p>
        </div>
      </div>
    )
  }

  if (games.length === 0) {
    return (
      <div className="product-list-container">
        <div className="empty-state">
          <div className="empty-icon">🔍</div>
          <h3>No se encontraron productos</h3>
          <p>Intenta ajustar tus filtros de búsqueda</p>
        </div>
      </div>
    )
  }



  return (
    <div className="product-list-container">
      <div className="results-header">
        <h2>Resultados ({games.length} productos)</h2>
      </div>

      <div className="product-grid">
        {games.map((games) => (
                <Card
                key={games.id}
                id={games.id}
                name={games.name}
                image={games.image}
                link = {games.link}
                originalPrice={games.price}
                discount={games.discount}               
                />
        ))}
      </div>
    </div>
  )
}
