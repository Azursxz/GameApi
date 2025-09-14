
import { useState } from "react"
import "./filterControl.css"

interface FilterControlsProps {
  filters?: {
    priceMin: number
    priceMax: number
    discountMin: number
    discountMax: number
  }
  sortBy?: "price-asc" | "price-desc" | "discount-asc" | "discount-desc" | "name-asc" | "name-desc"
  onFiltersChange?: (filters: any) => void
  onSortChange?: (
    sortBy: "price-asc" | "price-desc" | "discount-asc" | "discount-desc" | "name-asc" | "name-desc",
  ) => void
  onPageReset?: (page: number) => void;
}


export default function FilterControls({
  filters = { priceMin: 0, priceMax: 2000, discountMin: 0, discountMax: 100 },
  sortBy = "name-asc",
  onFiltersChange,
  onSortChange,
  onPageReset,
}: FilterControlsProps) {
  const [localFilters, setLocalFilters] = useState(filters)
  const [localSort, setLocalSort] = useState(sortBy)
  const sortOptions = [
    { value: "name-asc", label: "Nombre A-Z" },
    { value: "name-desc", label: "Nombre Z-A" },
    { value: "price-asc", label: "Precio: Menor a Mayor" },
    { value: "price-desc", label: "Precio: Mayor a Menor" },
    { value: "discount-asc", label: "Descuento: Menor a Mayor" },
    { value: "discount-desc", label: "Descuento: Mayor a Menor" },
  ]

  const handleInputChange = (field: string, value: number) => {
    const newFilters = { ...localFilters, [field]: value }
    setLocalFilters(newFilters)
    onFiltersChange?.(newFilters) // Solo llama si existe
    onPageReset?.(1)
  }

  const handleSortChangeLocal = (value: typeof localSort) => {
    setLocalSort(value)
    onSortChange?.(value)
    onPageReset?.(1)
  }

  const resetFilters = () => {
    const resetValues = {
      priceMin: 0,
      priceMax: 2000000,
      discountMin: 0,
      discountMax: 100,
    }
    setLocalFilters(resetValues)
    setLocalSort("name-asc")
    onFiltersChange?.(resetValues)
    onSortChange?.("name-asc")
    onPageReset?.(1)
  }

  
  return (
    <div className="filter-controls">
      <div className="filter-header">
        <h3 className="filter-title">Filtros</h3>
        <div className="filter-buttons">
            <button className="reset-button" onClick={resetFilters}>
             Resetear Todo
            </button>
           <button className="search-button"> 
             Buscar Juegos
           </button>
        </div>
      </div>

      <div className="filter-groups">
        {/* Sección de Ordenamiento */}
        <div className="filter-group sort-group">
          <h4 className="filter-group-title">Ordenar por</h4>
          <div className="sort-select-container">
            <select className="sort-select" value={sortBy} onChange={(e) => handleSortChangeLocal(e.target.value as any)}>
              {sortOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
            <div className="select-arrow">
              <svg width="12" height="8" viewBox="0 0 12 8" fill="none">
                <path
                  d="M1 1L6 6L11 1"
                  stroke="currentColor"
                  strokeWidth="2"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
              </svg>
            </div>
          </div>
        </div>

        {/* Sección de Filtro por Precio */}
        <div className="filter-group">
          <h4 className="filter-group-title">Rango de Precio</h4>
          <div className="range-inputs">
            <div className="input-group">
              <label className="input-label">Mínimo</label>
              <input
                type="number"
                className="filter-input"
                value={localFilters.priceMin}
                onChange={(e) => handleInputChange("priceMin", Number(e.target.value))}
                min="0"
                placeholder="0"
              />
            </div>
            <div className="input-group">
              <label className="input-label">Máximo</label>
              <input
                type="number"
                className="filter-input"
                value={localFilters.priceMax}
                onChange={(e) => handleInputChange("priceMax", Number(e.target.value))}
                min="0"
                placeholder="20000"
              />
            </div>
          </div>
        </div>

        {/* Sección de Filtro por Descuento */}
        <div className="filter-group">
          <h4 className="filter-group-title">Rango de Descuento</h4>
          <div className="range-inputs">
            <div className="input-group">
              <label className="input-label">Mínimo</label>
              <input
                type="number"
                className="filter-input"
                value={localFilters.discountMin}
                onChange={(e) => handleInputChange("discountMin", Number(e.target.value))}
                min="0"
                max="100"
                placeholder="0"
              />
            </div>
            <div className="input-group">
              <label className="input-label">Máximo</label>
              <input
                type="number"
                className="filter-input"
                value={localFilters.discountMax}
                onChange={(e) => handleInputChange("discountMax", Number(e.target.value))}
                min="0"
                max="100"
                placeholder="100"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
