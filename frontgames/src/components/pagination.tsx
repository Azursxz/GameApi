import { useState } from "react";
import "./pagination.css";

interface PaginationProps {
  totalItems: number;
  itemsPerPage: number;
  onPageChange: (page: number) => void;
  actualPage:number;
}

export default function Pagination({ totalItems, itemsPerPage, onPageChange , actualPage}: PaginationProps) {
  const [currentPage, setCurrentPage] = useState(1);
  const totalPages = Math.ceil(totalItems / itemsPerPage);

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
    onPageChange(page);
  };

  const pages = [];
  for (let i = 1; i <= totalPages; i++) {
    pages.push(i);
  }

  return (
    <div className="pagination">
      <button
        onClick={() => handlePageChange(Math.max(currentPage - 1, 1))}
        disabled={actualPage === 1}
        className="pagination-button"
      >
        Prev
      </button>

      <button
        onClick={() => handlePageChange(Math.min(currentPage + 1, totalPages))}
        disabled={actualPage === totalPages}
        className="pagination-button"
      >
        Next
      </button>
    </div>
  );
}