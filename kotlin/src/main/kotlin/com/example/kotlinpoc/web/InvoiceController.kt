package com.example.kotlinpoc.web

import com.example.kotlinpoc.persistence.Invoice
import com.example.kotlinpoc.persistence.InvoiceRepository
import com.example.kotlinpoc.persistence.LineItem
import org.springframework.web.bind.annotation.*
import java.util.*

@RestController
class InvoiceController(private val repository: InvoiceRepository) {

    @GetMapping("/invoice")
    fun findAll() = repository.findAll()

    @GetMapping("/invoice/{id}")
    fun findOne(@PathVariable id: UUID) = repository.findById(id);

    @PostMapping("/invoice")
    fun postCustomer(@RequestBody newInvoice: NewInvoice): Invoice {
        return repository.save(from(newInvoice))
    }

    fun from(newInvoice: NewInvoice): Invoice {
        val invoice = Invoice(
                status = newInvoice.status,
                amountDue = newInvoice.amountDue,
                lineItems = null
        )

        var lineItems = newInvoice.lineItems?.map {
            LineItem(
                    description = it.description,
                    quantity = it.quantity,
                    unitAmount = it.unitAmount,
                    lineAmount = it.lineAmount,
                    invoice = invoice
            )
        }

        invoice.lineItems = lineItems
        return invoice

    }

}