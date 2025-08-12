"use client"

import Card from "./card"
import "./ProductList.css"

interface Product {
  id: number
  name: string
  price: number
  discount: number
  link: string
  image: string
}

interface ProductListProps {
  products: Product[]
  loading?: boolean
}

export default function ProductList({ products, loading = false }: ProductListProps) {
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

  if (products.length === 0) {
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
        <h2>Resultados ({products.length} productos)</h2>
      </div>

      <div className="product-grid">
        {products.map((product) => (
                <Card
                key={product.id}
                id={product.id}
                name={product.name}
                image={product.image}
                link = {product.link}
                originalPrice={product.price}
                discount={product.discount}               
                />
        ))}
      </div>
    </div>
  )
}
