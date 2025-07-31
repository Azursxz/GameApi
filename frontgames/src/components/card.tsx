import { useState } from "react"
import "./card.css"

interface GameCardProps {
  id: number,
  name: string
  image: string
  originalPrice: number
  discount?: number
}


export default function MatrixCard({id, name, image, originalPrice, discount }: GameCardProps) {

  const [isExpanded, setIsExpanded] = useState(false)
  const [selectedImage, setSelectedImage] = useState(0)


   const openModal = () => {
    setIsExpanded(true)
    document.body.style.overflow = "hidden"
  }

  const closeModal = () => {
    setIsExpanded(false)
    document.body.style.overflow = "unset"
  }

  return (
    <div className="card-game">
 
      <div className="image-container">
        <img src={image} alt={name} width={400} height={300} className="image-game"/>
        {discount && <div className="discount-badge">-{discount}%</div>}
      </div>

      <div className="content-container">
        <h3 className="name-game">{name}</h3>
        <div className="price-game">
          {discount ? (
            <>
              <span className="original-price">${originalPrice}</span>
              <span className="discount-price">${originalPrice*discount}</span>
            </>
          ) : (
            <span className="current-price">${originalPrice}</span>
          )}
        </div>

        <div className="buttons-container">
        <button className="button-xbox">VER EN XBOX</button>
        <button onClick={openModal} className="button-ver">VER MAS</button>
        </div>

      </div>
    </div>

  )
}