//import { useState } from "react"
import "./card.css"

interface GameCardProps {
  id: number,
  name: string
  image: string
  link: string
  originalPrice: number
  discount?: number
}


export default function Card({id, name, image, originalPrice, discount,link }: GameCardProps) {

//  const [isExpanded, setIsExpanded] = useState(false)
 // const [selectedImage, setSelectedImage] = useState(0)

/*
   const openModal = () => {
    setIsExpanded(true)
    document.body.style.overflow = "hidden"
  }

  const closeModal = () => {
    setIsExpanded(false)
    document.body.style.overflow = "unset"
  }

          <button onClick={openModal} className="button-ver">VER MAS</button>

*/

  const calculateDiscountedPrice = (originalPrice: number, discount: number) => {
    return originalPrice - (originalPrice * discount) / 100
  }


  return (
    <div className="card-game">
 
      <div className="image-container">
        <img src={image} alt={name} width={400} height={300} className="image-game"/>
      </div>

      <div className="content-container">
        <h3 className="name-game">{name}</h3>
        <div className="price-game">
          {discount ? (
            <>
            <div className="discount-container">
              <span className="original-price">ARS${originalPrice.toFixed(2)}</span>
              <span className="discount-badge">-{discount}%</span>
            </div>
            <div className="price-discount-container">
              <span className="discount-price">ARS${calculateDiscountedPrice(originalPrice,discount).toFixed(2)}</span>
            </div>

            </>
          ) : (
            <span className="current-price">${originalPrice.toFixed(2)}</span>
          )}
        </div>

        <div className="buttons-container">
        <button className="button-xbox">
          <a className="xbox-link" href={link} target="_blank" rel="noopener noreferrer">Ver en XBOX</a>
        </button>
        

        </div>

      </div>
    </div>

  )
}