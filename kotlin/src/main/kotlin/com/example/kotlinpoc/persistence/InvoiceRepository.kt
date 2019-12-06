package com.example.kotlinpoc.persistence


 import com.example.kotlinpoc.persistence.Invoice
 import org.springframework.data.repository.CrudRepository
 import org.springframework.stereotype.Repository
 import java.util.*

@Repository
interface InvoiceRepository : CrudRepository<Invoice, UUID> {
//    fun findAllByOrderByAddedAtDesc(): Iterable<Invoice>
}