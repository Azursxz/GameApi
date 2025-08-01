import { useState } from "react"
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
  const [selectedImage, setSelectedImage] = useState(0)

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
              <span className="original-price">${originalPrice.toFixed(2)}</span>
              <span className="discount-price">${(originalPrice*(1-discount/100)).toFixed(2)}</span>
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